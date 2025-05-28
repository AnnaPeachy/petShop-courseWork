using System;
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
            try
            {
                if (!File.Exists(path))
                    return new List<Product>();

                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки товаров: {ex.Message}");
                return new List<Product>();
            }
        }

        public static List<Service> LoadServices(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return new List<Service>();

                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<Service>>(json) ?? new List<Service>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки списка услуг: {ex.Message}");
                return new List<Service>();
            }
        }
    }
}
