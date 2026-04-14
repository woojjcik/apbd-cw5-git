namespace LegacyRenewalApp;

public class PlatinumDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan, bool use)
    {
        notes = "";
        decimal discountAmount = 0m;

        if (customer.Segment == "Platinum")
        { 
            discountAmount = baseAmount * 0.15m;
            notes += "platinum discount; ";
        }

        return discountAmount;
    }
}