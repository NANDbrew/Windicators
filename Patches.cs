﻿using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using WindiBridge;

namespace Windicators
{
    [HarmonyPatch(typeof(PrefabsDirectory), "PopulateShipItems")]
    internal static class ItemTest
    {
        internal static void Prefix()
        {
            if (AssetTools.bundle == null) AssetTools.LoadAssetBundles();

            foreach (var item in AssetTools.itemPrefabs)
            {
                if (item.Key >= PrefabsDirectory.instance.directory.Length)
                {
                    Array.Resize(ref PrefabsDirectory.instance.directory, item.Key + 5);
                    Debug.Log("Windicators: Resized directory to " + PrefabsDirectory.instance.directory.Length + " to accommodate " + item.Value.name);
                }
                if (PrefabsDirectory.instance.directory[item.Key] == null)
                {
                    PrefabsDirectory.instance.directory[item.Key] = item.Value;
                }
                else
                {
                    Debug.LogWarning($"Windicators: Prefab at index {item.Key} already exists in directory, skipping {item.Value.name}");
                }
            }
        }
    }
    [HarmonyPatch(typeof(IslandStreetlightsManager), "Awake")]
    internal static class IslandHorizonPatches
    {
        [HarmonyPostfix]
        internal static void Postfix(IslandStreetlightsManager __instance)
        {
            if (AssetTools.bundle == null) AssetTools.LoadAssetBundles();
            int index = __instance.gameObject.GetComponent<IslandSceneryScene>().parentIslandIndex;
#if DEBUG
            Debug.Log("Windicators: patching shopkeeper on island " + index);
#endif
            if (AssetTools.shopKeepers.ContainsKey(index))
            {
                var shopkeeper = UnityEngine.Object.Instantiate(AssetTools.shopKeepers[index], __instance.transform);
                shopkeeper.shopPrefab.transform.parent = __instance.transform;

                Debug.Log($"Windicators: Adding shopkeeper for island {index}");
            }
            else
            {
#if DEBUG
                Debug.LogWarning($"Windicators: No shopkeeper found for island {index}");
#endif
            }
        }
    }


    [HarmonyPatch(typeof(ShipItem))]
    internal static class ShipItemPatches
    {
        [HarmonyPatch("OnEnterInventory")]
        [HarmonyPostfix]
        public static void EnterPatch(ShipItem __instance)
        {
            //Debug.Log("into the pocket");
            __instance.GetComponent<InvSwitcher>()?.OnEnterInventory();
        }
        [HarmonyPatch("OnLeaveInventory")]
        [HarmonyPostfix]
        internal static void ExitPatch(ShipItem __instance)
        {
            //Debug.Log("out of the pocket");
            __instance.GetComponent<InvSwitcher>()?.OnLeaveInventory();
        }
        [HarmonyPatch("OnDrop")]
        [HarmonyPostfix]
        public static void DropPatch(ShipItemHangable __instance)
        {
            //Debug.Log("dropped");
            __instance.GetComponent<InvSwitcher>()?.OnDrop();
        }
        [HarmonyPatch("OnPickup")]
        [HarmonyPostfix]
        internal static void PickupPatch(ShipItem __instance)
        {
            //Debug.Log("picked up");
            __instance.GetComponent<InvSwitcher>()?.OnPickup();
        }
        [HarmonyPatch("ReturnToShopPos")]
        [HarmonyPostfix]
        internal static void ReturnToShopPatch(ShipItem __instance)
        {
            //Debug.Log("drop");
            __instance.GetComponent<InvSwitcher>()?.OnDrop();
        }
    }
}
