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
        public const string PLUGIN_VERSION = "0.0.1";

        //--settings--
        //internal ConfigEntry<bool> someSetting;


        private void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);

            //someSetting = Config.Bind("Settings", "Some setting", false);
        }
    }
}
