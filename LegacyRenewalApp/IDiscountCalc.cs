namespace LegacyRenewalApp;

public interface IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes, SubscriptionPlan subscriptionPlan, bool useLoyalityPoints);
}
