using System.Globalization;
using System.Xml;

namespace AssetTracking;

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