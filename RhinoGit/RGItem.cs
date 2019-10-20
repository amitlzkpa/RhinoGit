using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoGit
{
   class RGItem : IComparable
   {
      public Guid id;
      public string geometry;
      public string attributes;

      public int CompareTo(object obj)
      {
         return ((IComparable)id).CompareTo(obj);
      }
   }
}
