using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace petShop_courseWork.Model
{
    // Конвертер для сериализации и десериализации абстрактного ShopItem
    public class ShopItemJsonConverter : JsonConverter<ShopItem>
    {
        // Метод для десериализации объекта ShopItem
        public override ShopItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                // Проверяем наличие поля "Type", по которому определяется конкретный тип
                if (!root.TryGetProperty("Type", out JsonElement typeElement))
                    throw new JsonException("Отсутствует поле Type");

                string type = typeElement.GetString();

                // В зависимости от значения поля "Type" десериализуем в нужный класс
                if (type == "Product")
                {
                    return JsonSerializer.Deserialize<Product>(root.GetRawText(), options);
                }
                else if (type == "Service")
                {
                    return JsonSerializer.Deserialize<Service>(root.GetRawText(), options);
                }
                else
                {
                    throw new JsonException($"Неизвестный тип: {type}");
                }
            }
        }

        // Метод для сериализации объекта ShopItem
        public override void Write(Utf8JsonWriter writer, ShopItem value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Вставляем специальное поле "Type", чтобы сохранить информацию о типе объекта
            if (value is Product product)
            {
                writer.WriteString("Type", "Product");

                // Сериализуем все свойства объекта Product
                var productJson = JsonSerializer.SerializeToElement(product, options);
                foreach (var prop in productJson.EnumerateObject())
                {
                    prop.WriteTo(writer);
                }
            }
            else if (value is Service service)
            {
                writer.WriteString("Type", "Service");

                // Сериализуем все свойства объекта Service
                var serviceJson = JsonSerializer.SerializeToElement(service, options);
                foreach (var prop in serviceJson.EnumerateObject())
                {
                    prop.WriteTo(writer);
                }
            }
            else
            {
                throw new NotSupportedException("Неизвестный тип ShopItem");
            }

            writer.WriteEndObject();
        }
    }
}

