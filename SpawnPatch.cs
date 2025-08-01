using HarmonyLib;
using UnityEngine;

namespace Windicators
{
    internal static class SpawnPatch
    {
        [HarmonyPostfix]
        public static void UpdatePatch()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    SpawnItem(501, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    SpawnItem(502, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    SpawnItem(503, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    SpawnItem(504, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    SpawnItem(505, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {
                    SpawnItem(506, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {
                    SpawnItem(507, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {
                    SpawnItem(508, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
                else if (Input.GetKeyDown(KeyCode.Keypad9))
                {
                    SpawnItem(509, Refs.ovrCameraRig.position + Refs.ovrCameraRig.forward, Quaternion.identity);
                }
            }
        }
        public static void SpawnItem(int itemIndex, Vector3 position, Quaternion rotation)
        {
            if (SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory != null)
            {
                GameObject obj = UnityEngine.Object.Instantiate(SaveLoadManager.instance.GetComponent<PrefabsDirectory>().directory[itemIndex], position, rotation);
                obj.GetComponent<ShipItem>().sold = true;
                obj.GetComponent<SaveablePrefab>().RegisterToSave();
                //obj.GetComponent<Good>()?.RegisterAsMissionless();
            }
        }

    }
}
