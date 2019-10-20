using System;
using System.IO;
using System.Collections.Generic;

using Rhino;
using Rhino.Commands;

using LibGit2Sharp;

namespace RhinoGit
{
   public class Commit : Command
   {


      static Commit _instance;
      public Commit()
      {
         _instance = this;
      }

      ///<summary>The only instance of the Commit command.</summary>
      public static Commit Instance
      {
         get { return _instance; }
      }

      public override string EnglishName
      {
         get { return "Commit"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {
         string commitMessgae = string.Empty;

         try
         {
            Rhino.Input.Custom.GetString gs = new Rhino.Input.Custom.GetString();
            gs.SetCommandPrompt("Commit message");
            gs.Get();
            commitMessgae = gs.StringResult();
         } catch(Exception ex)
         {
            RhinoApp.WriteLine(ex.ToString());
            return Result.Failure;
         }

         if (string.IsNullOrEmpty(commitMessgae)) return Result.Cancel;


         RGIndex rgi = new RGIndex(doc.Objects);
         string path = Path.Combine(RhinoGitCommand.repo.Info.WorkingDirectory, RhinoGitCommand.indexFileName);
         string json = RGIndexSerializer.GetTextFromIndex(rgi);
         File.WriteAllText(path, json);

         RhinoGitCommand.repo.Index.Add(RhinoGitCommand.indexFileName);
         RhinoGitCommand.repo.Index.Write();

         Signature author = new Signature("amit", "@amitlzkpa", DateTime.Now);
         Signature committer = author;

         LibGit2Sharp.Commit commit = RhinoGitCommand.repo.Commit(commitMessgae, author, committer);

         return Result.Success;
      }



   }
}