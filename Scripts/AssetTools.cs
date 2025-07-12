using System.IO;
using UnityEngine;

namespace Windicators
{
    internal class AssetTools
    {
        public static AssetBundle bundle;
        //const string assetDir = "";
        const string assetFile = "windicators";
        static string combined;
        public static void LoadAssetBundles()    //Load the bundle
        {
            string dataPath = Directory.GetParent(Plugin.instance.Info.Location).FullName;
            //string firstTry = Path.Combine(dataPath, assetDir, assetFile);
            string secondTry = Path.Combine(dataPath, assetFile);
            //else { Debug.LogError("TowableBoats: can't find asset file"); return; }
            combined = secondTry;
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
