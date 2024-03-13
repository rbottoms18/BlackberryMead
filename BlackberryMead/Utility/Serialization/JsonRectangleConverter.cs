using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlackberryMead.Utility.Serialization
{
    internal class JsonRectangleConverter : JsonConverter<Rectangle>
    {
        public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Rectangle rect = new Rectangle();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return rect;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string? propertyName = reader.GetString();

                // Get the value.
                reader.Read();

                if (propertyName == "X")
                {
                    rect.X = reader.GetInt16();
                }
                else if (propertyName == "Y")
                {
                    rect.Y = reader.GetInt16();
                }
                else if (propertyName == "Width")
                {
                    rect.Width = reader.GetInt16();
                }
                else if (propertyName == "Height")
                {
                    rect.Height = reader.GetInt16();
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber("X", value.X);
            writer.WriteNumber("Y", value.Y);
            writer.WriteNumber("Width", value.Width);
            writer.WriteNumber("Height", value.Height);

            writer.WriteEndObject();
        }
    }
}
