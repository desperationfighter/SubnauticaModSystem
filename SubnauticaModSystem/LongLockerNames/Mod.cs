using System;
using System.IO;
using System.Linq;
using UnityEngine;

//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//Ingame Slider
using SMLHelper.V2.Handlers;

/*
xcopy $(TargetPath) D:\SteamLibrary\steamapps\common\Subnautica\QMods\$(ProjectName)\ /q /y
xcopy $(ProjectDir)mod.json D:\SteamLibrary\steamapps\common\Subnautica\QMods\$(ProjectName)\ /q /y
xcopy $(ProjectDir)Assets D:\SteamLibrary\steamapps\common\Subnautica\QMods\$(ProjectName)\Assets\ /q /y /i

xcopy $(TargetPath) D:\EpicGames\Subnautica\QMods\$(ProjectName)\ /q /y
xcopy $(ProjectDir)mod.json D:\EpicGames\Subnautica\QMods\$(ProjectName)\ /q /y
xcopy $(ProjectDir)Assets D:\EpicGames\Subnautica\QMods\$(ProjectName)\Assets\ /q /y /i
*/

namespace LongLockerNames
{
	[QModCore]
	public static class Mod
	{
		private static Assembly myAssembly = Assembly.GetExecutingAssembly();
		private static string ModPath = Path.GetDirectoryName(myAssembly.Location);
		internal static string AssetsFolder = Path.Combine(ModPath, "Assets");
		internal static Config Config { get; private set; }

		[QModPatch]
		public static void Patch()
        {
			Config = OptionsPanelHandler.RegisterModOptions<Config>();
			IngameMenuHandler.RegisterOnSaveEvent(Config.Save);

			Harmony.CreateAndPatchAll(myAssembly, $"RandyKnapp_{myAssembly.GetName().Name}");

			QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, "LongLockerNames Patched");
		}

		public static void PrintObject(GameObject obj, string indent = "")
		{
			Console.WriteLine(indent + "[[" + obj.name + "]]:");
			Console.WriteLine(indent + "{");
			Console.WriteLine(indent + "  Components:");
			Console.WriteLine(indent + "  {");
			var lastC = obj.GetComponents<Component>().Last();
			foreach (var c in obj.GetComponents<Component>())
			{
				Console.WriteLine(indent + "    " + c.ToString().Replace(obj.name, "").Trim());
			}
			Console.WriteLine(indent + "  }");
			Console.WriteLine(indent + "  Children:");
			Console.WriteLine(indent + "  {");
			foreach (Transform child in obj.transform)
			{
				PrintObject(child.gameObject, indent + "    ");
			}
			Console.WriteLine(indent + "  }");
			Console.WriteLine(indent + "}");
		}
	}
}