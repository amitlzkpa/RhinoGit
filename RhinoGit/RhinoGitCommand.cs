using System;
using System.IO;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using LibGit2Sharp;

namespace RhinoGit
{
   public class RhinoGitCommand : Command
   {
      public static string gitPath;
      public static string indexFileName = "index.json";

      private static Repository _repo = null;
      public static Repository repo
      {
         get
         {
            if (_repo == null)
            {
               gitPath = Repository.Init("C:\\DATA\\amit\\rgit\\gits\\alpha\\");
               _repo = new Repository(RhinoGitCommand.gitPath);
            }
            return _repo;
         }
      }

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

         //// write
         //RGIndex rgi = new RGIndex(doc.Objects);
         //string path = "C:\\DATA\\amit\\rgit\\files\\rgi.json";
         //string json = RGIndexSerializer.GetTextFromIndex(rgi);
         //File.WriteAllText(path, json);

         // read
         //string path = "C:\\DATA\\amit\\rgit\\files\\rgi.json";
         //string json = File.ReadAllText(path);
         //RGIndex rgi = RGIndexSerializer.ReadJson(json);
         //foreach (Guid id in rgi.items.Keys)
         //{
         //   GeometryBase gb = (rgi.items[id] as RGItem).geometry;
         //   doc.Objects.Add(gb);
         //}

         return Result.Success;
      }
   }
}
