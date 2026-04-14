namespace LegacyRenewalApp;

public class SilverDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan)
    {
        notes = "";
        decimal discountAmount = 0m;

        if (customer.Segment == "Silver")
        {
            discountAmount = baseAmount * 0.05m;
            notes += "silver discount; ";
        }
        
        return discountAmount;
    }
}