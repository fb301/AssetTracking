namespace AssetTracking.Models;

public class Asset
{
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Location { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Currency { get; set; }
    public double Price { get; set; }
    public double PriceUSD { get; set; }

    public Asset(string type, string brand, string model, string location, DateTime purchasedate, string currency, double price, double priceUSD)
    {
        Type = type;
        Brand = brand;
        Model = model;
        Location = location;
        PurchaseDate = purchasedate;
        Currency = currency;
        Price = price;
        PriceUSD = priceUSD;
    }

    public string CheckDate()
    {
        DateTime expireDate = PurchaseDate.AddYears(3);

        if (expireDate <= DateTime.Now.AddMonths(3))
        {
            return "Red";
        }
        else if (expireDate <= DateTime.Now.AddMonths(6))
        {
            return "Yellow";
        }
        else
        {
            return "Green";
        }
    }

    public override string ToString() =>
        $"{Type} - {Brand} - {Model} (Location: {Location}, Purchased: {PurchaseDate}, USD: {PriceUSD}, {Currency}: {Price})";
}