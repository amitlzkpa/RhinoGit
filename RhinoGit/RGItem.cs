using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoGit
{
   class RGItem
   {
      public Guid id;
      public string geometry;
      public string attributes;

      public bool EqualGeometry(RGIndex otherItem)
      {
         return false;
      }

      public bool EqualAttributes(RGIndex otherItem)
      {
         return false;
      }

   }
}
