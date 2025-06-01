using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace petShop_courseWork.Model
{
    public class ShopItemJsonConverter : JsonConverter<ShopItem>
    {
        public override ShopItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("Type", out JsonElement typeElement))
                    throw new JsonException("Отсутствует поле Type");

                string type = typeElement.GetString();

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


        public override void Write(Utf8JsonWriter writer, ShopItem value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is Product product)
            {
                writer.WriteString("Type", "Product");

                var productJson = JsonSerializer.SerializeToElement(product, options);
                foreach (var prop in productJson.EnumerateObject())
                {
                    prop.WriteTo(writer);
                }
            }
            else if (value is Service service)
            {
                writer.WriteString("Type", "Service");

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
