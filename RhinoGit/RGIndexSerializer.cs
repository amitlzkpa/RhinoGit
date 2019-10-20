using System;
using System.IO;

using Rhino.Geometry;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
         RGIndex rgi = existingValue as RGIndex;
         object oo = reader;
         string tt = reader.ReadAsString();
         return rgi;
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
            writer.WriteValue(GetStringFromGeometry(rgitem.geometry));
            writer.WriteEndObject();
         }
         writer.WriteEndArray();
         writer.WriteEndObject();

      }

      private static string GetStringFromGeometry(GeometryBase gb)
      {
         System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
         using (var ms = new MemoryStream())
         {
            bf.Serialize(ms, gb);
            return Convert.ToBase64String(ms.ToArray());
         }
      }

      private static GeometryBase GetGeometryFromString(string txt)
      {
         System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
         using (var ms = new MemoryStream(Convert.FromBase64String(txt)))
         {
            return bf.Deserialize(ms) as GeometryBase;
         }
      }


      public static RGIndex ReadJson(string jsonText)
      {
         RGIndex rgi = new RGIndex();
         JObject jo = JObject.Parse(jsonText);
         JArray items = jo["items"] as JArray;
         foreach(dynamic jt in items)
         {
            Guid id = Guid.Parse(jt.id.ToString());
            string geomTxt = jt["geometry"].ToString();
            GeometryBase geom = GetGeometryFromString(geomTxt);
            rgi.Add(id, geom);
         }
         return rgi;
      }


   }
}
