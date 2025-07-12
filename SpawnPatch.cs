using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Windicators
{
    [HarmonyPatch(typeof(PlayerCrouching))]
    internal static class SpawnPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void UpdatePatch()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                //Debug.Log("trying to instantiate object");
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(501, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(502, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(503, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(504, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
                {
                    SpawnItem(505, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
            }
        }
        public static GameObject SpawnItem(int itemIndex, Vector3 position, Quaternion rotation)
        {

            GameObject obj = UnityEngine.Object.Instantiate(SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory[itemIndex], position, rotation);
            obj.GetComponent<ShipItem>().sold = true;
            obj.GetComponent<SaveablePrefab>().RegisterToSave();
            //obj.GetComponent<Good>()?.RegisterAsMissionless();
            return obj;
        }

    }
}
