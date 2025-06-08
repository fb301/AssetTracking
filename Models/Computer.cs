namespace AssetTracking.Models;

public class Computer : Asset
{
    public Computer(string brand, string model, string location, DateTime purchasedate, double price, string currency, double priceUSD)
        : base("Computer", brand, model, location, purchasedate, currency, price, priceUSD)
    {
    }
}