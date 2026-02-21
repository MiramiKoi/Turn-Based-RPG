using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Editor.ScriptedImporters
{
    [ScriptedImporter(1, "lua")]
    public class LuaImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var text = File.ReadAllText(ctx.assetPath);
            var asset = new TextAsset(text);

            ctx.AddObjectToAsset("LuaText", asset);
            ctx.SetMainObject(asset);
        }
    }
}