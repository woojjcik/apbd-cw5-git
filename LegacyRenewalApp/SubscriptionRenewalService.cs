using System;
using System.Collections.Generic;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly IEnumerable<IDiscountCalc> _discountCalc;
        private readonly ISupportFeeCalc _supportFeeCalc;
        private readonly IPaymentFee _paymentFee;
        private readonly ITaxCalculator _taxCalculator;

        public SubscriptionRenewalService(IEnumerable<IDiscountCalc> discountCalc, ISupportFeeCalc supportFeeCalc,
            IPaymentFee paymentFee, ITaxCalculator taxCalculator)
        {
            _discountCalc = discountCalc;
            _supportFeeCalc = supportFeeCalc;
            _paymentFee = paymentFee;
            _taxCalculator = taxCalculator;

        }
        public SubscriptionRenewalService() : this(
            new IDiscountCalc[]
            {
                new SilverDisc(),
                new GoldDisc(),
                new PlatinumDisc(),
                new EducationDisc(),
                new YearsDisc(),
                new SeatCountDisc(),
                new LoyalityDisc()
            }, new FeeCalc(),
            new PaymentFeeCalc(), new TaxCalc()
            )
        {}
        
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Customer id must be positive");
            }

            if (string.IsNullOrWhiteSpace(planCode))
            {
                throw new ArgumentException("Plan code is required");
            }

            if (seatCount <= 0)
            {
                throw new ArgumentException("Seat count must be positive");
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method is required");
            }

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customerRepository = new CustomerRepository();
            var planRepository = new SubscriptionPlanRepository();

            var customer = customerRepository.GetById(customerId);
            var plan = planRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }

            string notes = string.Empty;
            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            decimal discountAmount = 0m;

            foreach (var v in _discountCalc)
            {
             discountAmount += v.calucalteDiscount(baseAmount, customer, seatCount, out string nnotes, plan, useLoyaltyPoints);
             notes += nnotes;
            }

            decimal subtotalAfterDiscount = baseAmount - discountAmount;
            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes += "minimum discounted subtotal applied; ";
            }

            decimal supportFee = 0m;
            if (includePremiumSupport)
            {
                supportFee = _supportFeeCalc.calculateFee(normalizedPlanCode, out string nnotes);
                notes += nnotes;
            }

            decimal paymentFee = _paymentFee.calculatePaymentFee(normalizedPaymentMethod, 
                supportFee, subtotalAfterDiscount, out string ntes);
            notes += ntes;

            decimal taxRate = _taxCalculator.calculateTax(customer);

            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;
            decimal finalAmount = taxBase + taxAmount;

            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes += "minimum invoice amount applied; ";
            }

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            LegacyBillingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                LegacyBillingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }
    }
}
