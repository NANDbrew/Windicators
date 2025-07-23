using HarmonyLib;
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
            //if (PrefabsDirectory.instance.directory.Length < 522) Array.Resize(ref PrefabsDirectory.instance.directory, 522);
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
    [HarmonyPatch(typeof(IslandHorizon), "LoadIslandScene")]
    internal static class IslandHorizonPatches
    {
        [HarmonyPostfix]
        internal static void Postfix(IslandHorizon __instance)
        {
            if (AssetTools.bundle == null) AssetTools.LoadAssetBundles();
            Debug.Log("horizonPatches: patching shopkeeper");
            if (AssetTools.shopKeepers.ContainsKey(__instance.islandIndex))
            {
                __instance.StartCoroutine(AddShopKeeper(__instance.islandIndex, AssetTools.shopKeepers[__instance.islandIndex]));
#if DEBUG
                Debug.Log($"Windicators: Adding shopkeeper for island {__instance.islandIndex}");
#endif
            }
        }
        private static IEnumerator AddShopKeeper(int islandIndex, GameObject shopkeeper)
        {
            yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(islandIndex).isLoaded);
            var parent = SceneManager.GetSceneByBuildIndex(islandIndex).GetRootGameObjects()[0].transform;
            UnityEngine.Object.Instantiate(shopkeeper, parent.transform);

        }

    }


    [HarmonyPatch(typeof(ShipItem))]
    internal static class ShipItemPatches
    {
        [HarmonyPatch("OnEnterInventory")]
        [HarmonyPostfix]
        public static void EnterPatch(ShipItem __instance)
        {
            Debug.Log("into the pocket");
            //__instance.GetComponent<InvSwitcherWindCompass>()?.OnEnterInventory();
            __instance.GetComponent<InvSwitcher>()?.OnEnterInventory();//ToggleFlag(false);
        }
        [HarmonyPatch("OnLeaveInventory")]
        [HarmonyPostfix]
        internal static void ExitPatch(ShipItem __instance)
        {
            Debug.Log("out of the pocket");
            //__instance.GetComponent<InvSwitcherWindCompass>()?.OnLeaveInventory();
            __instance.GetComponent<InvSwitcher>()?.OnLeaveInventory();// ToggleFlag(true);
        }
        [HarmonyPatch("OnDrop")]
        [HarmonyPostfix]
        public static void DropPatch(ShipItemHangable __instance)
        {
            Debug.Log("dropped");
            __instance.GetComponent<InvSwitcher>()?.OnDrop();
        }
        [HarmonyPatch("OnPickup")]
        [HarmonyPostfix]
        internal static void PickupPatch(ShipItem __instance)
        {
            Debug.Log("picked up");
            __instance.GetComponent<InvSwitcher>()?.OnPickup();
        }
        [HarmonyPatch("ReturnToShopPos")]
        [HarmonyPostfix]
        internal static void ReturnToShopPatch(ShipItem __instance)
        {
            Debug.Log("drop");
            __instance.GetComponent<InvSwitcher>()?.OnDrop();
        }
    }
}
