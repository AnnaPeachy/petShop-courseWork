using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using petShop_courseWork.Model;

namespace petShop_courseWork.Services
{
    public class ProductLoader
    {
        public static List<Product> LoadProducts(string path)
        {
            if (!File.Exists(path)) return new List<Product>();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Product>>(json);
        }

        public static List<Service> LoadServices(string path)
        {
            if (!File.Exists(path)) return new List<Service>();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Service>>(json);
        }
    }
}
