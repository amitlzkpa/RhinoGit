using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Commands;

namespace RhinoGit
{
   public class RGIndex
   {
      SortedList items = new SortedList();

      public RGIndex(IEnumerable<RhinoObject> rhos)
      {
         foreach(RhinoObject rho in rhos)
         {
            Add(rho);
         }
      }

      public bool Add(RhinoObject rho)
      {
         RGItem ri = new RGItem();
         items.Add(ri.id, ri);
         return true;
      }

      public bool RemoveById(Guid id)
      {
         items.Remove(id);
         return true;
      }

      public bool RemoveByGeometry(GeometryBase geom)
      {
         Guid rid = Guid.Empty;
         GeometryBase a = null;
         foreach(RGItem ri in items)
         {
            //a = "convert to my geom";
            if (GeometryBase.GeometryEquals(a, geom))
            {
               rid = ri.id;
               break;
            }
         }
         if (rid == Guid.Empty) return false;
         RemoveById(rid);
         return true;
      }


   }
}