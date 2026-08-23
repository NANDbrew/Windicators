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

        public static Dictionary<int, GameObject> itemPrefabs = new Dictionary<int, GameObject>();
        public static Dictionary<int, ShopInfo> shopPrefabs = new Dictionary<int, ShopInfo>();
        //public static Dictionary<int, RolloverTumbler> tumblers = new Dictionary<int, RolloverTumbler>();

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
                        shopPrefabs.Add(info.parentIslandIndex, info);
#if DEBUG
                        Debug.Log($"Windicators: added {info.name} to directory");
#endif
                    }
/*                    else if (prefab.GetComponent<RolloverTumbler>() is RolloverTumbler tumbler)
                    {
                        if (tumbler.name == "plus_tumbler_M")
                        {
                            tumblers[92] = tumbler;
                        }
                        if (tumbler.name == "plus_tumbler_E")
                        {
                            tumblers[93] = tumbler;
                        }
                    }*/
                }

            }
            else { Debug.LogError("Couldn't find file!!"); }
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
                    var newShader = Shader.Find(shaderName);
                    if (newShader != null)
                    {
                        m.shader = newShader;

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
