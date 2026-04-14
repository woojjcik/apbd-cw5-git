namespace LegacyRenewalApp;

public interface ITaxCalculator
{
    public decimal calculateTax(Customer customer);
}