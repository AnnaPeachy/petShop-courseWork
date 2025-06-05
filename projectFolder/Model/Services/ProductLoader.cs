using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using petShop_courseWork.Model;


namespace petShop_courseWork.Services
{
    public class ProductLoader
    {
        private const string SessionFile = "Data/session.json";

        // Сохраняем текущее состояние в файл
        public static void SaveSession(SessionData data)
        {
            Directory.CreateDirectory("Data");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new ShopItemJsonConverter());

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(SessionFile, json);
        }

        // Загружаем сохранённую сессию
        public static SessionData LoadSession()
        {
            if (!File.Exists(SessionFile))
                return null;

            var options = new JsonSerializerOptions();
            options.Converters.Add(new ShopItemJsonConverter());

            string json = File.ReadAllText(SessionFile);
            return JsonSerializer.Deserialize<SessionData>(json, options);
        }

        // Загрузка списка товаров из файла
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

        // Загрузка списка услуг из файла
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

