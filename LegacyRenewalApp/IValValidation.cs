namespace LegacyRenewalApp;

public interface IValValidation
{
    public void validate(int customerId, string planCode, int seatCount, string paymentMethod);
}