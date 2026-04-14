namespace LegacyRenewalApp;

public class GoldDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan, bool use)
    {
        decimal discountAmount = 0m;
        notes = "";
        if (customer.Segment == "Gold")
        {
            discountAmount = baseAmount * 0.10m;
            notes += "gold discount; ";
        }

        return discountAmount;
    }
}