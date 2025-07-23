using PsychoticLab;
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
        public static Dictionary<int, GameObject> shopKeepers = new Dictionary<int, GameObject>();

        public static void LoadAssetBundles()    //Load the bundle
        {
            string dataPath = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            string secondTry = Path.Combine(dataPath, assetFile);

            string libSecondTry = Path.Combine(dataPath, libFile);
            if (File.Exists(libSecondTry)) 
            {
                Assembly.LoadFrom(libSecondTry);
                Debug.Log("WindiBridge loaded other successfully");
            }
            else { Debug.LogError("Failed to load WindiBridge!"); }

            if (File.Exists(secondTry))
            {
                bundle = AssetBundle.LoadFromFile(secondTry);
/*                shopkeeperA = bundle.LoadAsset("Assets/Windicators/windicator_seller_A.prefab") as GameObject;
                shopkeeperE = bundle.LoadAsset("Assets/Windicators/windicator_seller_E.prefab") as GameObject;
                shopkeeperM = bundle.LoadAsset("Assets/Windicators/windicator_seller_M.prefab") as GameObject;
*/
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
                        shopKeepers.Add(info.parentIslandIndex, prefab);
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
