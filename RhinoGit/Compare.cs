using System;
using Rhino;
using Rhino.Commands;

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
         // TODO: complete command.
         return Result.Success;
      }
   }
}