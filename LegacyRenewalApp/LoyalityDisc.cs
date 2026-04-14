namespace LegacyRenewalApp;

public class LoyalityDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan, bool useLoyaltyPoints)
    {
        decimal discountAmount = 0m;
        notes = "";
        
        if (useLoyaltyPoints && customer.LoyaltyPoints > 0)
        {
            int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
            discountAmount += pointsToUse;
            notes += $"loyalty points used: {pointsToUse}; ";
        }

        return discountAmount;
    }
}