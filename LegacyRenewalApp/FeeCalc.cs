namespace LegacyRenewalApp;

public class FeeCalc:ISupportFeeCalc
{
    public decimal calculateFee(string planCode, out string notes)
    {
        decimal supportFee = 0m;
        notes = "";
        
        if (planCode == "START")
        {
            supportFee = 250m;
        }
        else if (planCode == "PRO")
        {
            supportFee = 400m;
        }
        else if (planCode == "ENTERPRISE")
        {
            supportFee = 700m;
        }

        notes += "premium support included; ";
        
        return supportFee;
    }

}
