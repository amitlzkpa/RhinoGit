using System;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Commands;

namespace RhinoGit
{
   public class RGIndex
   {
      List<RGItem> items = new List<RGItem>();

      public RGIndex(IEnumerable<RhinoObject> rhos)
      {
         foreach(RhinoObject rho in rhos)
         {
            Add(rho);
         }
      }

      public bool Add(RhinoObject rho)
      {
         return false;
      }

      public bool RemoveById(Guid id)
      {
         return false;
      }

      public bool RemoveByGeometry(GeometryBase geom)
      {
         return false;
      }


   }
}