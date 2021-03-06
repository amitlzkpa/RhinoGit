﻿using System;
using System.Collections;
using System.Collections.Generic;

using Rhino.Geometry;
using Rhino.DocObjects;

using Newtonsoft.Json.Serialization;

namespace RhinoGit
{
   public class RGIndex
   {
      public SortedList items = new SortedList();

      public RGIndex() { }

      public RGIndex(IEnumerable<RhinoObject> rhos)
      {
         foreach(RhinoObject rho in rhos)
         {
            Add(rho);
         }
      }

      public bool Add(Guid id, GeometryBase geom)
      {
         RGItem ri = new RGItem();
         ri.id = id;
         ri.geometry = geom;
         items.Add(ri.id, ri);
         return true;
      }

      public bool Add(RhinoObject rho)
      {
         RGItem ri = new RGItem();
         ri.id = rho.Id;
         ri.geometry = rho.Geometry;
         items.Add(ri.id, ri);
         return true;
      }

      public bool RemoveById(Guid id)
      {
         items.Remove(id);
         return true;
      }

   }
}