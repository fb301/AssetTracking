using AssetTracking.Models;
using AssetTracking.Services;

namespace AssetTracking;

public class Program
{
    public static void Main(string[] args)
    {
        // Load exchange rates from the European Central Bank
        ExchangeOperations.LoadRates();

        List<Asset> assets = new List<Asset>();

        Console.WriteLine("Do you want to add a new asset? (y/n)");
        while (Console.ReadLine()?.Trim().ToLower() == "y")
        {
            assets.Add(AssetService.CreateAssetFromUserInput());
            Console.WriteLine("Do you want to add another asset? (y/n)");
        }

        // Sort the assets by Location then by PurchaseDate
        assets.Sort((x, y) =>
        {
            int locationComparison = string.Compare(x.Location, y.Location, StringComparison.Ordinal);
            return locationComparison != 0 ? locationComparison : x.PurchaseDate.CompareTo(y.PurchaseDate);
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
    }
}