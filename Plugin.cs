using BepInEx;
using HarmonyLib;
using System.Reflection;

namespace Windicators
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.nandbrew.Windicators";
        public const string PLUGIN_NAME = "Windicators";
        public const string PLUGIN_VERSION = "1.0.2";

        //--settings--
        //internal ConfigEntry<bool> someSetting;
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
            //someSetting = Config.Bind("Settings", "Some setting", false);
        }
    }
}
