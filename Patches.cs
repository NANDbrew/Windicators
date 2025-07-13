using HarmonyLib;
using UnityEngine;
using System;
using WindiBridge;

namespace Windicators
{
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    internal static class ItemTest
    {
        internal static string[] prefabNames = { "telltale", "telltale 1", "telltale 2", "wind compass E", "telltale 3", "telltale 4", "telltale 5", "wind compass M" };
        internal static void Prefix()
        {
            Array.Resize(ref PrefabsDirectory.instance.directory, 512);
            AssetTools.LoadAssetBundles();
            foreach (var name in prefabNames)
            {
                var prefab = AssetTools.bundle.LoadAsset("Assets/Windicators/" + name + ".prefab") as GameObject;
                PrefabsDirectory.instance.directory[prefab.GetComponent<SaveablePrefab>().prefabIndex] = prefab;
            }
        }
    }
    [HarmonyPatch(typeof(ShipItem))]
    internal static class HangableItemPatches
    {
        [HarmonyPatch("OnEnterInventory")]
        [HarmonyPostfix]
        public static void EnterPatch(ShipItem __instance)
        {
            Debug.Log("into the pocket");
            __instance.GetComponentInChildren<WindFlag>(true)?.gameObject.SetActive(false);
        }
        [HarmonyPatch("OnLeaveInventory")]
        [HarmonyPostfix]
        internal static void ExitPatch(ShipItem __instance)
        {
            Debug.Log("out of the pocket");
            __instance.GetComponentInChildren<WindFlag>(true)?.gameObject.SetActive(true);
        }
    }

}
