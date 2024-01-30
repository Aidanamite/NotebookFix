using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Moonlighter.DLC.WandererDungeon;
using DigitalSunGames.Platforms;

namespace NotebookFix
{
    [BepInPlugin("Aidanamite.NotebookFix", "NotebookFix", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        internal static Assembly modAssembly = Assembly.GetExecutingAssembly();
        internal static string modName = $"{modAssembly.GetName().Name}";
        internal static string modDir = $"{Environment.CurrentDirectory}\\BepInEx\\{modName}";

        void Awake()
        {
            new Harmony($"com.Aidanamite.{modName}").PatchAll(modAssembly);
            Logger.LogInfo($"{modName} has loaded");
        }
    }

    [HarmonyPatch(typeof(WandererDLCController), "GetAvailableItemsToShow")]
    class Patch_WandererDLCController
    {
        static void Postfix(ref List<ItemMaster> __result)
        {
            int plus = GameManager.Instance.GetCurrentGamePlusLevel();
            if (plus > 0)
            {
                var r = __result;
                __result = ItemDatabase.GetItems((x) => r.Exists((y) => y.name == x.name), plus);
            }
        }
    }
}