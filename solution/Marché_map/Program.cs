using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Marché_map
{
    internal class Program
    {
        class Product
        {
            public int Location { get; set; }
            public string Producer { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public string Unit { get; set; }
            public double PricePerUnit { get; set; }
        }
        static List<Product> products = new List<Product> {
            new Product { Location = 1, Producer = "Bornand", ProductName = "Pommes", Quantity = 20, Unit = "kg", PricePerUnit = 5.50 },
            new Product { Location = 1, Producer = "Bornand", ProductName = "Poires", Quantity = 16, Unit = "kg", PricePerUnit = 5.50 },
            new Product { Location = 1, Producer = "Bornand", ProductName = "Pastèques", Quantity = 14, Unit = "pièce", PricePerUnit = 5.50 },
            new Product { Location = 1, Producer = "Bornand", ProductName = "Melons", Quantity = 5, Unit = "kg", PricePerUnit = 5.50 },
            new Product { Location = 2, Producer = "Dumont", ProductName = "Noix", Quantity = 20, Unit = "sac", PricePerUnit = 5.50 },
            new Product { Location = 2, Producer = "Dumont", ProductName = "Raisin", Quantity = 6, Unit = "kg", PricePerUnit = 5.50 },
            new Product { Location = 2, Producer = "Dumont", ProductName = "Pruneaux", Quantity = 13, Unit = "kg", PricePerUnit = 5.50 },
            new Product { Location = 2, Producer = "Dumont", ProductName = "Myrtilles", Quantity = 12, Unit = "kg", PricePerUnit = 5.50 },

        };
        static void Main(string[] args)
        {
            var i18n = new Dictionary<string, string>()
    {
        { "Pommes","Apples"},
        { "Poires","Pears"},
        { "Pastèques","Watermelons"},
        { "Melons","Melons"},
        { "Noix","Nuts"},
        { "Raisin","Grapes"},
        { "Pruneaux","Plums"},
        { "Myrtilles","Blueberries"}
    };

            // Filtre 1 : Producteur abrégé
            var filters = new List<string>();
            string filePath = "export.csv";
            filters.Add("Seller;Product;CA");

           filters.AddRange(products.Select(p =>
           {
               string seller = p.Producer.Substring(0, Math.Min(p.Producer.Length, 3)) +
                               (p.Producer.Length > 3 ? "..." + p.Producer[p.Producer.Length - 1] : "");

               string productName = i18n.ContainsKey(p.ProductName) ? i18n[p.ProductName] : p.ProductName;

               double CA = p.Quantity * p.PricePerUnit;

               return $"{seller};{productName};{CA}";
           }));

            // Write to CSV
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var line in filters)
                {
                    writer.WriteLine(line);
                }
            }

            // Print to console
            filters.ForEach(line => Console.WriteLine(line));

            Console.ReadLine();
        }
    }
}


