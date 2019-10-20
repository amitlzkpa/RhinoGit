using System;
using System.IO;

using Rhino.Geometry;

using Newtonsoft.Json;

namespace RhinoGit
{
   class RGIndexSerializer : JsonConverter
   {
      public override bool CanConvert(Type objectType)
      {
         return typeof(RGIndex).IsAssignableFrom(objectType);
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
         throw new NotImplementedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
         RGIndex rgi = value as RGIndex;
         if(rgi == null)
         {
            throw new InvalidCastException();
         }

         RGItem rgitem = null;

         writer.WriteStartObject();
         writer.WritePropertyName("items");
         writer.WriteStartArray();
         foreach(Guid rid in rgi.items.Keys)
         {
            rgitem = rgi.items[rid] as RGItem;
            writer.WriteStartObject();
            writer.WritePropertyName("id");
            writer.WriteValue(rgitem.id.ToString());
            writer.WritePropertyName("geometry");
            writer.WriteValue(GetGeometryBaseString(rgitem.geometry));
            writer.WriteEndObject();
         }
         writer.WriteEndArray();
         writer.WriteEndObject();

      }

      private string GetGeometryBaseString(GeometryBase gb)
      {
         System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
         using (var ms = new MemoryStream())
         {
            bf.Serialize(ms, gb);
            return Convert.ToBase64String(ms.ToArray());
         }
      }


   }
}
