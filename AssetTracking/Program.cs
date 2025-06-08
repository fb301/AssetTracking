using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;

// Load exchange rates from the European Central Bank
ExchangeOperations.LoadRates();
// Function to create an asset based on user input
static Asset CreateAssetFromUserInput()
{
    Console.WriteLine("What type of asset do you want to create? (1 for Phone, 2 for Computer):");
    string choice = Console.ReadLine()?.Trim() ?? "";

    while (choice != "1" && choice != "2")
    {
        Console.WriteLine("Please enter 1 for Phone or 2 for Computer:");
        choice = Console.ReadLine()?.Trim() ?? "";
    }

    Console.WriteLine("Enter brand:");
    string brand = Console.ReadLine()?.Trim() ?? "";
    while (string.IsNullOrWhiteSpace(brand))
    {
        Console.WriteLine("Brand cannot be empty. Please enter brand:");
        brand = Console.ReadLine()?.Trim() ?? "";
    }

    Console.WriteLine("Enter model:");
    string model = Console.ReadLine()?.Trim() ?? "";
    while (string.IsNullOrWhiteSpace(model))
    {
        Console.WriteLine("Model cannot be empty. Please enter model:");
        model = Console.ReadLine()?.Trim() ?? "";
    }

    Console.WriteLine("Choose office location, (1) Sweden (2) Japan (3) Denmark");
    string location = Console.ReadLine()?.Trim() ?? "";
    string currency = "USD"; // Default currency
    switch (location.ToLower())
    {
        case "1":
            location = "Sweden";
            currency = "SEK";
            break;
            case "2":
            location = "Japan";
            currency = "JPY";
            break;
            case "3":
            location = "Denmark";
            currency = "DKK";
            break;
    }
    while (string.IsNullOrWhiteSpace(location))
    {
        Console.WriteLine("Location is empty");
        location = Console.ReadLine()?.Trim() ?? "";
    }

    DateTime purchaseDate;
    while (true)
    {
        Console.WriteLine("Enter purchase date (YYYY-MM-DD):");
        if (DateTime.TryParse(Console.ReadLine(), out purchaseDate))
            break;
        Console.WriteLine("Invalid date format. Please use YYYY-MM-DD format.");
    }

    double price;
    double priceLocal = 0.0;
    while (true)
    {
        Console.WriteLine("Enter price in USD:");
        if (double.TryParse(Console.ReadLine(), out price) && price > 0)
            priceLocal = price * ExchangeOperations.ExchangeRate[currency] / ExchangeOperations.ExchangeRate["USD"];
            break;
        Console.WriteLine("Invalid price. Please enter a positive number.");
    }

    return choice == "1"
        ? new Phone(brand, model, location, purchaseDate, priceLocal, currency, price) 
        : new Computer(brand, model, location, purchaseDate, priceLocal, currency, price);
}

List<Asset> assets = new List<Asset>();

Console.WriteLine("Do you want to add a new asset? (y/n)");
while (Console.ReadLine()?.Trim().ToLower() == "y")
{
    assets.Add(CreateAssetFromUserInput());
    Console.WriteLine("Do you want to add another asset? (y/n)");
}


// Sort the assets by Location then by PurchaseDate
assets.Sort((x, y) =>
{
    // Compare by Location first
    int locationComparison = string.Compare(x.Location, y.Location, StringComparison.Ordinal);
    if (locationComparison != 0)
    {
        return locationComparison;
    }
    // If Type is the same, compare by PurchaseDate
    return x.PurchaseDate.CompareTo(y.PurchaseDate);
});

// Display the assets with color coding based on expiration date
foreach (var asset in assets)
{
    Console.ResetColor();
    switch (asset.CheckDate())
    {
        case "Red":
            Console.ForegroundColor = ConsoleColor.Red;
            break;
        case "Yellow":
            Console.ForegroundColor = ConsoleColor.Yellow;
            break;
    }
    Console.WriteLine(asset);
    Console.ResetColor();
    Console.WriteLine();
}

// Define the ExchangeOperations class to handle exchange rates
public class ExchangeOperations
{
    public static readonly Dictionary<string, double> ExchangeRate = new();

    public static void LoadRates()
    {

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");

        foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
        {
            ExchangeRate.Add(node.Attributes["currency"].Value, double.Parse(node.Attributes["rate"].Value, CultureInfo.InvariantCulture));
            Console.WriteLine($"Currency: {node.Attributes["currency"].Value}, Rate: {node.Attributes["rate"].Value}");
        }
    }
}

// Define the Asset class with properties and methods
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

    // (Keeping for future reference)
    // Uncomment this method if you want to allow updating the asset's location and recalculating the price in USD
    //public void UpdateLocation(string newLocation)
    //{
    //    string oldLocation = Location;
    //    Location = newLocation;
    //    Console.WriteLine($"Asset location updated from {oldLocation} to {Location}");

    //    switch (Location)
    //    {
    //        case "Sweden":
    //            PriceUSD = Price * ExchangeOperations.ExchangeRate["SEK"] / ExchangeOperations.ExchangeRate["USD"];
    //            break;
    //        case "Denmark":
    //            PriceUSD = Price * ExchangeOperations.ExchangeRate["USD"] / ExchangeOperations.ExchangeRate["DKK"];
    //            break;
    //        case "Japan":
    //            PriceUSD = Price * ExchangeOperations.ExchangeRate["USD"] / ExchangeOperations.ExchangeRate["JPY"];
    //            break;
    //    }
    //}

    // Check if the asset is within 3 months of expiring, within 6 months, or more than 6 months away
    public string CheckDate()
    {
        DateTime expireDate = PurchaseDate.AddYears(3);

        if (expireDate <= DateTime.Now.AddMonths(3))
        {
            return "Red"; // Within 3 months of expiring
        }
        else if (expireDate <= DateTime.Now.AddMonths(6))
        {
            return "Yellow"; // Within 6 months of expiring
        }
        else
        {
            return "Green"; // More than 6 months away
        }
    }

    public override string ToString() =>
        $"{Type} - {Brand} - {Model} (Location: {Location}, Purchased: {PurchaseDate}, USD: {PriceUSD}, {Currency}: {Price})";

}



// Define specific asset types inheriting from Asset
class Phone : Asset
{
    public Phone(string brand, string model, string location, DateTime purchasedate, double price, string currency, double priceUSD)
        : base("Phone", brand, model, location, purchasedate, currency, price, priceUSD)
    {
    }
}
// Define specific asset types inheriting from Asset
class Computer : Asset
{
public Computer(string brand, string model, string location, DateTime purchasedate, double price, string currency, double priceUSD)
        : base("Computer", brand, model, location, purchasedate, currency, price, priceUSD)
    {
    }
}