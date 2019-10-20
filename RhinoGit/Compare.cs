using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
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

            int idx_NEW = doc.Layers.Add("NEW", System.Drawing.Color.Green);
            int idx_DEL = doc.Layers.Add("DELETED", System.Drawing.Color.Red);
            int idx_MOD = doc.Layers.Add("MODIFIED", System.Drawing.Color.Purple);

            var commit = RhinoGitCommand.repo.Commits.Single(c => c.Sha.ToString().StartsWith(srcHash.ToString()));
            var file = commit[RhinoGitCommand.indexFileName];

            var blob = file.Target as Blob;
            using (var content = new StreamReader(blob.GetContentStream(), Encoding.UTF8))
            {
               var fileContent = content.ReadToEnd();

               RGIndex srcIdx = RGIndexSerializer.GetIndexFromText(fileContent);
               RGIndex newItemsList = new RGIndex(doc.Objects);
               RGIndex currIdx = new RGIndex(doc.Objects);

               foreach(Guid srcId in srcIdx.items.Keys)
               {
                  RGItem srcItem = srcIdx.items[srcId] as RGItem;
                  if (!currIdx.items.ContainsKey(srcItem.id))
                  {
                     // deleted
                     ObjectAttributes delAttr = new ObjectAttributes();
                     delAttr.LayerIndex = idx_DEL;
                     doc.Objects.Add(srcItem.geometry, delAttr);
                  } else
                  {
                     GeometryBase srcGeom = srcItem.geometry;
                     GeometryBase compGeom = (currIdx.items[srcItem.id] as RGItem).geometry;
                     if (!GeometryBase.GeometryEquals(srcGeom, compGeom))
                     {
                        // modified
                        ObjectAttributes modAttr = new ObjectAttributes();
                        modAttr.LayerIndex = idx_MOD;
                        doc.Objects.Add(srcGeom, modAttr);
                        doc.Objects.Add(compGeom, modAttr);
                        newItemsList.RemoveById(srcItem.id);
                        continue;
                     }
                     newItemsList.RemoveById(srcItem.id);
                  }
               }

               foreach(Guid newItemId in newItemsList.items.Keys)
               {
                  // new
                  RGItem newItem = newItemsList.items[newItemId] as RGItem;
                  ObjectAttributes newAttr = new ObjectAttributes();
                  newAttr.LayerIndex = idx_NEW;
                  doc.Objects.Add(newItem.geometry, newAttr);
               }



            }
         }

         return Result.Success;
      }






   }
}