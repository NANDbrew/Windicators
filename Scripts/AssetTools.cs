using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using WindiBridge;

namespace Windicators
{
    internal class AssetTools
    {
        public static AssetBundle bundle;
        const string assetFile = "windicators";
        const string libFile = "WindiBridge.dll";

        public static GameObject shopkeeperA;
        public static GameObject shopkeeperM;
        public static GameObject shopkeeperE;
        public static Dictionary<int, GameObject> itemPrefabs = new Dictionary<int, GameObject>();
        public static Dictionary<int, ShopInfo> shopKeepers = new Dictionary<int, ShopInfo>();

        public static void LoadAssetBundles()
        {
            string dataPath = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            string secondTry = Path.Combine(dataPath, assetFile);

            string libSecondTry = Path.Combine(dataPath, libFile);
            if (File.Exists(libSecondTry)) 
            {
                Assembly.LoadFrom(libSecondTry);
                string version = System.Diagnostics.FileVersionInfo.GetVersionInfo(libSecondTry).FileVersion;
                Debug.Log($"Windicators: Loaded WindiBridge v{version}");
            }
            else { Debug.LogError("Windicators: Failed to load WindiBridge!"); }

            if (File.Exists(secondTry))
            {
                bundle = AssetBundle.LoadFromFile(secondTry);

                Shader standard = Shader.Find("Standard");
                Shader surface = Shader.Find("Particles/Standard Surface");
                foreach (var prefab in bundle.LoadAllAssets<GameObject>())
                {
                    // this is a hack to fix objects rendering over fog. REMOVE AS SOON AS POSSIBLE
                    if (prefab.name == "MatLib")
                    {
                        var child1 = prefab.transform.GetChild(0).GetComponent<Renderer>();
                        for (int i = 0; i < child1.sharedMaterials.Length; i++)
                        {
                            var mat = new Material(standard);
                            mat.CopyPropertiesFromMaterial(child1.sharedMaterials[i]);
                            child1.sharedMaterials[i] = mat;
                        }
                        var child2 = prefab.transform.GetChild(1).GetComponent<Renderer>();
                        for (int i = 0; i < child2.sharedMaterials.Length; i++)
                        {
                            var mat = new Material(surface);
                            mat.CopyPropertiesFromMaterial(child2.sharedMaterials[i]);
                            child2.sharedMaterials[i] = mat;
                        }
                    }
                    if (prefab.GetComponent<SaveablePrefab>() is SaveablePrefab saveable)
                    {
                        itemPrefabs.Add(saveable.prefabIndex, prefab);
#if DEBUG
                        Debug.Log($"Windicators: Added {prefab.name} to asset directory");
#endif
                    }
                    else if (prefab.GetComponent<ShopInfo>() is ShopInfo info)
                    {
                        shopKeepers.Add(info.parentIslandIndex, info);
#if DEBUG
                        Debug.Log($"Windicators: added {info.name} to directory");
#endif
                    }

                }
            }
            else { Debug.LogError("BULLSHITT!!"); }
            if (bundle == null)
            {
                Debug.LogError("Windicators: Bundle not loaded! Did you place it in the correct folder?");
            }
            else { Debug.Log("Windicators: loaded bundle " + bundle.ToString()); }
        }
    }
}
