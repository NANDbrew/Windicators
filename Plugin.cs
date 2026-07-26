using BepInEx;
using HarmonyLib;
using System.Reflection;
using BepInEx.Configuration;

namespace Windicators
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.nandbrew.Windicators";
        public const string PLUGIN_NAME = "Windicators";
        public const string PLUGIN_VERSION = "1.1.0";

        //--settings--
        internal ConfigEntry<bool> knotsConversion;
        internal static Plugin instance;

        private void Awake()
        {
            instance = this;
            Harmony harmony = new Harmony(PLUGIN_ID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

#if DEBUG
            MethodInfo original = AccessTools.Method(typeof(PlayerCrouching), "Update");
            MethodInfo patch = AccessTools.Method(typeof(SpawnPatch), "UpdatePatch");
            harmony.Patch(original, new HarmonyMethod(patch));
#endif
            AssetTools.LoadAssetBundles();
            //SceneManager.sceneLoaded += AddShopItems.SceneLoaded;

            knotsConversion = Config.Bind("Settings", "Knots Conversion", false, new ConfigDescription("Convert wind speed to chiplog knots"));

            knotsConversion.SettingChanged += (sender, args) => { WindiBridge.Anemometer.knotsConversion = knotsConversion.Value ? 1.865f : 1f; };
        }
    }
}
