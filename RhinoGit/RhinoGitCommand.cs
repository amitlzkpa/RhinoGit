﻿using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

using Newtonsoft.Json;

namespace RhinoGit
{
   public class RhinoGitCommand : Command
   {
      public RhinoGitCommand()
      {
         // Rhino only creates one instance of each command class defined in a
         // plug-in, so it is safe to store a refence in a static property.
         Instance = this;
      }

      ///<summary>The only instance of this command.</summary>
      public static RhinoGitCommand Instance
      {
         get; private set;
      }

      ///<returns>The command name as it appears on the Rhino command line.</returns>
      public override string EnglishName
      {
         get { return "RhinoGitCommand"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {
         RhinoApp.WriteLine($"The {EnglishName} working good!.");

         RGIndex rgi = new RGIndex(doc.Objects);
         string json = JsonConvert.SerializeObject(rgi, Formatting.Indented, new RGIndexSerializer());

         string path = "C:\\DATA\\amit\\rgit\\files\\rgi.json";
         File.WriteAllText(path, json);

         return Result.Success;
      }
   }
}
