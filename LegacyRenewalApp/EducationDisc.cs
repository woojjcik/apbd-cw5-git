namespace LegacyRenewalApp;

public class EducationDisc:IDiscountCalc
{
    public decimal calucalteDiscount(decimal baseAmount, Customer customer, int seatCount, out string notes,
        SubscriptionPlan subscriptionPlan)
    {
        decimal discountAmount = 0m;
        notes = "";
        if (subscriptionPlan.IsEducationEligible && customer.Segment == "Education")
        {
            discountAmount += baseAmount * 0.20m;
        }

        notes = "education discount; ";
        return discountAmount;
    }
}