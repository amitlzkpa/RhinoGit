using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

         writer.WriteStartObject();
         writer.WritePropertyName("items");
         writer.WriteStartArray();
         foreach(RGItem ri in rgi.items)
         {
            writer.WriteStartObject();
            writer.WritePropertyName("id");
            writer.WriteValue(ri.id.ToString());
            writer.WriteEndObject();
         }
         writer.WriteEndArray();
         writer.WriteEndObject();

      }
   }
}
