namespace LegacyRenewalApp;

public class YearsDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan, bool use)
    {
        decimal discountAmount = 0m;
        notes = "";
        if (customer.YearsWithCompany >= 5)
        {
            discountAmount += baseAmount * 0.07m;
            notes += "long-term loyalty discount; ";
        }
        else if (customer.YearsWithCompany >= 2)
        {
            discountAmount += baseAmount * 0.03m;
            notes += "basic loyalty discount; ";
        }

        return discountAmount;
    }
}