using HarmonyLib;
using UnityEngine;
using System;
using Windicators.Scripts;
namespace Windicators
{
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    internal static class ItemTest
    {
        internal static string[] prefabNames = { "telltale", "telltale 1", "telltale 2", "wind compass E", "telltale 3" };
        internal static void Prefix()
        {
            Array.Resize(ref PrefabsDirectory.instance.directory, 512);
            AssetTools.LoadAssetBundles();
            foreach (var name in prefabNames)
            {
                var prefab = AssetTools.bundle.LoadAsset("Assets/Windicators/" + name + ".prefab") as GameObject;
                PrefabsDirectory.instance.directory[prefab.GetComponent<SaveablePrefab>().prefabIndex] = prefab;
                if (prefab.GetComponentInChildren<WindDirectionDisplay>() is WindDirectionDisplay disp)
                {
                    var newComp = disp.gameObject.AddComponent<Weathervane>();
                    //if (name == "telltale 2" || name == "telltale 3") newComp.pivotLocal = false;
                    if (disp.transform.parent.GetComponent<KeepVertical>() is KeepVertical comp2)
                    {
                        comp2.gameObject.AddComponent<AutoLevel>();
                        Component.Destroy(comp2);
                    }
                    Component.Destroy(disp);
                }
                if (prefab.transform.GetChild(0).GetComponent<WindClothSimple>() is WindClothSimple wcs)
                {
                    wcs.gameObject.AddComponent<WindFlag>().multiplier = wcs.staticMultiplier;
                    Component.Destroy(wcs);
                }
                //prefab.AddComponent<WindToggler>();
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
