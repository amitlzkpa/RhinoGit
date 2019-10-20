using System;
using System.Collections.Generic;

using Rhino;
using Rhino.Commands;

using LibGit2Sharp;

namespace RhinoGit
{
   public class Branch : Command
   {
      static Branch _instance;
      public Branch()
      {
         _instance = this;
      }

      ///<summary>The only instance of the Branch command.</summary>
      public static Branch Instance
      {
         get { return _instance; }
      }

      public override string EnglishName
      {
         get { return "Branch"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {

         string branchName = string.Empty;

         try
         {
            Rhino.Input.Custom.GetString gs = new Rhino.Input.Custom.GetString();
            gs.SetCommandPrompt("Name:");
            gs.Get();
            branchName = gs.StringResult();
         }
         catch (Exception ex)
         {
            RhinoApp.WriteLine(ex.ToString());
            return Result.Failure;
         }

         if (string.IsNullOrEmpty(branchName)) return Result.Cancel;

         using (RhinoGitCommand.repo)
         {
            RhinoGitCommand.repo.CreateBranch(branchName);
         }

         return Result.Success;
      }
   }
}