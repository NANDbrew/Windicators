using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    else if (prefab.GetComponent<RolloverTumbler>() is RolloverTumbler tumbler)
                    {
                        if (tumbler.name == "plus_tumbler_M")
                        {
                            var chiplog = PrefabsDirectory.instance.directory[92].transform.Find("chiplog_M");
                            tumbler.sourceNeedle = chiplog.Find("pointer_002");
                            var t2 = GameObject.Instantiate(tumbler, chiplog, false);
                        }
                        if (tumbler.name == "plus_tumbler_E")
                        {
                            var chiplog = PrefabsDirectory.instance.directory[93].transform.Find("chiplog_E");
                            tumbler.sourceNeedle = chiplog.Find("pointer_001");
                            var t2 = GameObject.Instantiate(tumbler, chiplog, false);
                        }
                    }
                }

                //var points = GameObject.Instantiate(itemPrefabs[514].transform.Find(), 
            }
            else { Debug.LogError("BULLSHITT!!"); }
            if (bundle == null)
            {
                Debug.LogError("Windicators: Bundle not loaded! Did you place it in the correct folder?");
            }
            else 
            { 
                Debug.Log("Windicators: loaded bundle " + bundle.ToString());

                // stupid hack to fix fogless shader
                var mats = bundle.LoadAllAssets(typeof(Material));
                foreach (Material m in mats.Cast<Material>())
                {
                    var shaderName = m.shader.name;
                    //Debug.LogWarning("trying to refresh shader: " + shaderName + " in material " + m.name);
                    var newShader = Shader.Find(shaderName);
                    if (newShader != null)
                    {
                        m.shader = newShader;
                        //Debug.LogWarning("refreshed shader: " + shaderName + " in material " + m.name);

                    }
                    else
                    {
                        Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + m.name);
                    }
                }
            }
        }
    }
}
