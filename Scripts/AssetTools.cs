using System.IO;
using System.Reflection;
using UnityEngine;

namespace Windicators
{
    internal class AssetTools
    {
        public static AssetBundle bundle;
        const string assetFile = "windicators";
        const string libFile = "WindiBridge.dll";
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

            if (File.Exists(secondTry)) bundle = AssetBundle.LoadFromFile(secondTry);
            else { Debug.LogError("BULLSHITT!!"); }
            if (bundle == null)
            {
                Debug.LogError("Windicators: Bundle not loaded! Did you place it in the correct folder?");
            }
            else { Debug.Log("Windicators: loaded bundle " + bundle.ToString()); }
        }
    }
}
