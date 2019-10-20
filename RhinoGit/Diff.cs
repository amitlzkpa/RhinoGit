using System;
using System.Linq;
using System.Collections.Generic;

using Rhino;
using Rhino.Commands;

using LibGit2Sharp;

namespace RhinoGit
{
   public class Diff : Command
   {
      static Diff _instance;
      public Diff()
      {
         _instance = this;
      }

      ///<summary>The only instance of the Diff command.</summary>
      public static Diff Instance
      {
         get { return _instance; }
      }

      public override string EnglishName
      {
         get { return "Diff"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {


         string result;
         using (RhinoGitCommand.repo)
         {
            List<LibGit2Sharp.Commit> CommitList = new List<LibGit2Sharp.Commit>();
            foreach (LogEntry entry in RhinoGitCommand.repo.Commits.QueryBy(RhinoGitCommand.indexFileName).ToList())
            {
               CommitList.Add(entry.Commit);
            }
            CommitList.Add(null); // Added to show correct initial add

            int ChangeDesired = 0; // Change difference desired
            var repoDifferences = RhinoGitCommand.repo.Diff.Compare<Patch>((Equals(CommitList[ChangeDesired + 1], null)) ? null : CommitList[ChangeDesired + 1].Tree, (Equals(CommitList[ChangeDesired], null)) ? null : CommitList[ChangeDesired].Tree);
            PatchEntryChanges file = null;
            try { file = repoDifferences.First(e => e.Path == RhinoGitCommand.indexFileName); }
            catch { } // If the file has been renamed in the past- this search will fail
            if (!Equals(file, null))
            {
               result = file.Patch;
               RhinoApp.WriteLine(result.ToString());
            }


         }


         return Result.Success;
      }
   }
}