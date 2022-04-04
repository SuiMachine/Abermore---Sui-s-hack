using BepInEx;
using HarmonyLib;
using HarmonyLib.Tools;
using System.Globalization;

namespace AbemoreSuisHack
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Harmony HarmonyInstance { get; private set; }
        public static BepInEx.Logging.ManualLogSource log;
        public static readonly CultureInfo EnforcedCulture = new CultureInfo("en-GB"); //Developer is British, use what works for them

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is being loaded!");
            HarmonyInstance = new Harmony("local.abermorehack.suimachine");
            log = Logger;
#if DEBUG
            HarmonyFileLog.Enabled = true;
#else
            HarmonyFileLog.Enabled = false;
#endif
            CultureInfo.CurrentCulture = EnforcedCulture;
            CultureInfo.DefaultThreadCurrentCulture = EnforcedCulture;
            Hacks.QoL.QoL_EdgeBorderDisabler.use = Config.Bind("Quality_of_Life", "DisableEdgeBorders", true);
            Hacks.Quality_of_life.QoL_FieldOfView.lookFOV = Config.Bind("Quality_of_Life", "DesiredFOV", 60f);
            Hacks.Optimisation.Optimisation_MeshCombinerInit.use = Config.Bind("Optimisation", "ComineMeshes", false);
            Hacks.Optimisation.DisableOutdoorCamera.use = Config.Bind("Optimisation", "DisableOutdoorCamera", false);
            Hacks.Graphics.Unoptimised.enableShadows = Config.Bind("Graphics", "EnableShadows", true);
            Hacks.Graphics.Unoptimised.disableCameraOptimisations = Config.Bind("Graphics", "DisableCameraOptimisations", true);


            HarmonyInstance.PatchAll();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
