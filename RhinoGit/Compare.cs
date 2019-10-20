using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Rhino;
using Rhino.Commands;

using LibGit2Sharp;

namespace RhinoGit
{
   public class Compare : Command
   {
      static Compare _instance;
      public Compare()
      {
         _instance = this;
      }

      ///<summary>The only instance of the Compare command.</summary>
      public static Compare Instance
      {
         get { return _instance; }
      }

      public override string EnglishName
      {
         get { return "Compare"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {

         string srcHash = string.Empty;

         try
         {
            Rhino.Input.Custom.GetString gs = new Rhino.Input.Custom.GetString();
            gs.SetCommandPrompt("Source commit hash:");
            gs.Get();
            srcHash = gs.StringResult();
         }
         catch (Exception ex)
         {
            RhinoApp.WriteLine(ex.ToString());
            return Result.Failure;
         }

         if (string.IsNullOrEmpty(srcHash)) return Result.Cancel;

         using (RhinoGitCommand.repo)
         {
            foreach(LibGit2Sharp.Commit cm in RhinoGitCommand.repo.Commits)
            {
               RhinoApp.WriteLine(cm.ToString());
            }

            var commit = RhinoGitCommand.repo.Commits.Single(c => c.Sha.ToString().StartsWith(srcHash.ToString()));
            var file = commit[RhinoGitCommand.indexFileName];

            var blob = file.Target as Blob;
            using (var content = new StreamReader(blob.GetContentStream(), Encoding.UTF8))
            {
               var fileContent = content.ReadToEnd();
               RhinoApp.WriteLine(fileContent);
            }
         }

         return Result.Success;
      }
   }
}