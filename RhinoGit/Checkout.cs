using System;
using System.IO;

using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;

using LibGit2Sharp;

namespace RhinoGit
{
   public class Checkout : Command
   {
      static Checkout _instance;
      public Checkout()
      {
         _instance = this;
      }

      ///<summary>The only instance of the Checkout command.</summary>
      public static Checkout Instance
      {
         get { return _instance; }
      }

      public override string EnglishName
      {
         get { return "Checkout"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {

         string checkoutRef = string.Empty;

         try
         {
            Rhino.Input.Custom.GetString gs = new Rhino.Input.Custom.GetString();
            gs.SetCommandPrompt("Branch name:");
            gs.Get();
            checkoutRef = gs.StringResult();
         }
         catch (Exception ex)
         {
            RhinoApp.WriteLine(ex.ToString());
            return Result.Failure;
         }

         if (string.IsNullOrEmpty(checkoutRef)) return Result.Cancel;

         using (RhinoGitCommand.repo)
         {
            var branch = RhinoGitCommand.repo.Branches[checkoutRef];

            if (branch == null)
            {
               // repository return null object when branch not exists
               return Result.Failure;
            }

            LibGit2Sharp.Branch currentBranch = Commands.Checkout(RhinoGitCommand.repo, branch);
            foreach(RhinoObject ro in doc.Objects)
            {
               doc.Objects.Delete(ro);
            }

            string fileContent = File.ReadAllText(Path.Combine(RhinoGitCommand.gitPath, RhinoGitCommand.indexFileName));
            RGIndex srcIdx = RGIndexSerializer.GetIndexFromText(fileContent);

            foreach (Guid srcId in srcIdx.items.Keys)
            {
               RGItem srcItem = srcIdx.items[srcId] as RGItem;
               doc.Objects.Add(srcItem.geometry);
            }


         }

         return Result.Success;
      }


   }
}