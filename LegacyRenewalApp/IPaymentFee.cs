namespace LegacyRenewalApp;

public interface IPaymentFee
{
    public decimal calculatePaymentFee(string normalizedPaymentMethod,
        decimal supportFee, decimal subtotalAfterDiscount,  out string notes);
}