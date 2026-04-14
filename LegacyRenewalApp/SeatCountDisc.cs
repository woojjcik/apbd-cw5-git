namespace LegacyRenewalApp;

public class SeatCountDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan, bool use)
    {
        decimal discountAmount = 0m;
        notes = "";
        if (seatCount >= 50)
        {
            discountAmount += baseAmount * 0.12m;
            notes += "large team discount; ";
        }
        else if (seatCount >= 20)
        {
            discountAmount += baseAmount * 0.08m;
            notes += "medium team discount; ";
        }
        else if (seatCount >= 10)
        {
            discountAmount += baseAmount * 0.04m;
            notes += "small team discount; ";
        }
        
        return (discountAmount);
    }
}