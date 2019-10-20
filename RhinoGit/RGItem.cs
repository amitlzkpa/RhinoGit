using System;
using System.Collections.Generic;

using Rhino.Geometry;

namespace RhinoGit
{
   class RGItem : IComparable
   {
      public Guid id;
      public GeometryBase geometry;

      public int CompareTo(object obj)
      {
         return ((IComparable)id).CompareTo(obj);
      }
   }
}
