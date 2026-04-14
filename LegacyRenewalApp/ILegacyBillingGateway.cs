namespace LegacyRenewalApp;

public interface ILegacyBillingGateway
{
    public void SaveInvoice(RenewalInvoice invoice);

    public void SendEmail(string email, string subject, string body);
}