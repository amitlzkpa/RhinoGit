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

      string indexFileName = "index.json";
      string gitPath;
      Repository repo;



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
            Rhino.Input.Custom.GetString s = new Rhino.Input.Custom.GetString();
            commitMessgae = s.Get().ToString();
         } catch(Exception ex)
         {
            commitMessgae = string.Empty;
         }

         if (string.IsNullOrEmpty(commitMessgae)) return Result.Cancel;

         CreateIfNotThere();
         repo = new Repository(gitPath);


         RGIndex rgi = new RGIndex(doc.Objects);
         string path = Path.Combine(repo.Info.WorkingDirectory, indexFileName);
         string json = RGIndexSerializer.GetTextFromIndex(rgi);
         File.WriteAllText(path, json);

         repo.Index.Add(indexFileName);
         repo.Index.Write();

         Signature author = new Signature("James", "@jugglingnutcase", DateTime.Now);
         Signature committer = author;

         LibGit2Sharp.Commit commit = repo.Commit(commitMessgae, author, committer);

         return Result.Success;
      }


      private void CreateIfNotThere()
      {
         gitPath = Repository.Init("C:\\DATA\\amit\\rgit\\gits\\alpha\\");
      }

   }
}