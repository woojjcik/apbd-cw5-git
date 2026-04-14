namespace LegacyRenewalApp;

public interface ISupportFeeCalc
{
    public decimal calculateFee(string planCode, out string notes);
}