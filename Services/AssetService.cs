using AssetTracking.Models;

namespace AssetTracking.Services;

// AssetService is responsible for creating assets based on user input
public static class AssetService
{
    public static Asset CreateAssetFromUserInput()
    {

        // Prompt user for asset type
        Console.WriteLine("What type of asset do you want to create? (1 for Phone, 2 for Computer):");
        string choice = Console.ReadLine()?.Trim() ?? "";

        while (choice != "1" && choice != "2")
        {
            Console.WriteLine("Please enter 1 for Phone or 2 for Computer:");
            choice = Console.ReadLine()?.Trim() ?? "";
        }

        // Ensure brand is not empty
        Console.WriteLine("Enter brand:");
        string brand = Console.ReadLine()?.Trim() ?? "";
        while (string.IsNullOrWhiteSpace(brand))
        {
            Console.WriteLine("Brand cannot be empty. Please enter brand:");
            brand = Console.ReadLine()?.Trim() ?? "";
        }


        // Ensure model is not empty
        Console.WriteLine("Enter model:");
        string model = Console.ReadLine()?.Trim() ?? "";
        while (string.IsNullOrWhiteSpace(model))
        {
            Console.WriteLine("Model cannot be empty. Please enter model:");
            model = Console.ReadLine()?.Trim() ?? "";
        }


        // Choose office location and currency
        Console.WriteLine("Choose office location, (1) Sweden (2) Japan (3) Denmark");
        string location = Console.ReadLine()?.Trim() ?? "";
        string currency = "USD";
        switch (location)
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

        // Ask for purchase date in YYYY-MM-DD format
        DateTime purchaseDate;
        while (true)
        {
            Console.WriteLine("Enter purchase date (YYYY-MM-DD):");
            if (DateTime.TryParse(Console.ReadLine(), out purchaseDate))
                break;
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD format.");
        }

        // Take price in USD and convert it to the local currency
        double price;
        double priceLocal = 0.0;
        while (true)
        {
            Console.WriteLine("Enter price in USD:");
            if (double.TryParse(Console.ReadLine(), out price) && price > 0)
            {
                priceLocal = price * ExchangeOperations.ExchangeRate[currency] / ExchangeOperations.ExchangeRate["USD"];
                break;
            }
            Console.WriteLine("Invalid price. Please enter a positive number.");
        }

        return choice == "1"
            ? new Phone(brand, model, location, purchaseDate, priceLocal, currency, price)
            : new Computer(brand, model, location, purchaseDate, priceLocal, currency, price);
    }
}