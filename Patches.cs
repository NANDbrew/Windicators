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
/*            var chiplog_E = PrefabsDirectory.instance.directory[93].transform.Find("chiplog/chiplog_E");
            foreach (var tumbler in AssetTools.tumblers)
            {
                if (tumbler.Key == 92)
                {
                    var chiplog = PrefabsDirectory.instance.directory[92].transform.Find("chiplog/chiplog_M");
                    tumbler.Value.sourceNeedle = chiplog.Find("pointer_002");
                    tumbler.Value.transform.SetParent(chiplog, false);
                    tumbler.Value.transform.localEulerAngles = new Vector3(0, 90, 0);
                    //GameObject.Instantiate(tumbler.Value, chiplog, false);

                }
                else if (tumbler.Key == 93)
                {
                    var chiplog = PrefabsDirectory.instance.directory[93].transform.Find("chiplog/chiplog_E");
                    tumbler.Value.sourceNeedle = chiplog.Find("pointer_001");
                    tumbler.Value.transform.SetParent(chiplog, false);
                    tumbler.Value.transform.localEulerAngles = new Vector3(0, 90, 0);
                    //GameObject.Instantiate(tumbler.Value, chiplog, false);

                }
            }*/

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
/*                if (index == 15)
                {
                    AddShopItems.MakeShopItem("shop item (999)", __instance.transform, new Vector3(-68f, 3.3f, 44.1f), new Vector3(2f, 142f, 359.8f), AssetTools.itemPrefabs[514]);
                }
                else if (index == 9)
                {
                    AddShopItems.MakeShopItem("shop item (999)", __instance.transform, new Vector3(-79.8f, 4.6f, -546.5f), new Vector3(75f, 45.4f, 78.5f), AssetTools.itemPrefabs[512]);
                }*/
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
        public static void DropPatch(ShipItem __instance)
        {
            //Debug.Log("dropped");
            if (__instance.gameObject.layer != 26)
            { 
                __instance.GetComponent<InvSwitcher>()?.OnDrop(); 
            }
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

    [HarmonyPatch(typeof(CrateInventory))]
    internal static class ShipItemPatches2
    {
        [HarmonyPatch("InsertItem")]
        [HarmonyPostfix]
        public static void EnterPatch(ShipItem item)
        {
            //Debug.Log("into the pocket");
            item.GetComponent<InvSwitcher>()?.OnEnterInventory();
        }
        [HarmonyPatch("WithdrawItem")]
        [HarmonyPostfix]
        internal static void ExitPatch(ShipItem item)
        {
            //Debug.Log("out of the pocket");
            item.GetComponent<InvSwitcher>()?.OnLeaveInventory();
        }
    }

    [HarmonyPatch(typeof(ShipItemHammer))]
    internal static class HammerPatch
    {
        [HarmonyPatch("CanNail")]
        [HarmonyPostfix]
        public static void CanNailPatch(ShipItem item, ref bool __result)
        {
            if (item.GetComponent<InvSwitcher>()?.anemometer != null)
            {
                __result = true;
            }
        }

    }
}
