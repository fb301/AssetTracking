namespace AssetTracking.Models;

public class Phone : Asset
{
    public Phone(string brand, string model, string location, DateTime purchasedate, double price, string currency, double priceUSD)
        : base("Phone", brand, model, location, purchasedate, currency, price, priceUSD)
    {
    }
}