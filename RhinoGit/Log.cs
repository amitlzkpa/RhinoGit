using System;
using System.Linq;

using Rhino;
using Rhino.Commands;

using LibGit2Sharp;

namespace RhinoGit
{
   public class Log : Command
   {
      static Log _instance;
      public Log()
      {
         _instance = this;
      }

      ///<summary>The only instance of the Log command.</summary>
      public static Log Instance
      {
         get { return _instance; }
      }

      public override string EnglishName
      {
         get { return "Log"; }
      }

      protected override Result RunCommand(RhinoDoc doc, RunMode mode)
      {
         var RFC2822Format = "ddd dd MMM HH:mm:ss yyyy K";

         foreach (LibGit2Sharp.Commit c in RhinoGitCommand.repo.Commits.Take(10))
         {
            RhinoApp.WriteLine(string.Format("commit {0}", c.Id));

            if (c.Parents.Count() > 1)
            {
               RhinoApp.WriteLine("Merge: {0}",
                   string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
            }

            RhinoApp.WriteLine(string.Format("Author: {0} <{1}>", c.Author.Name, c.Author.Email));
            RhinoApp.WriteLine("Date:   {0}", c.Author.When.ToString(RFC2822Format, System.Globalization.CultureInfo.InvariantCulture));
            RhinoApp.WriteLine(c.Message);
            RhinoApp.WriteLine();
         }
         return Result.Success;
      }
   }
}