using BepInEx.Configuration;
using HarmonyLib;

namespace AbemoreSuisHack.Hacks.Graphics
{
	[HarmonyPatch]
	public class Unoptimised
	{
		public static ConfigEntry<bool> disableCameraOptimisations;
		public static ConfigEntry<bool> enableShadows;

		[HarmonyPrefix]
		[HarmonyPatch(typeof(PlayerCameraOptimization), "Update")]
		public static bool DetourPlayerCameraOptimisations()
		{
			if(Hacks.Optimisation.DisableOutdoorCamera.use.Value)
			{
				if(Player.bodyCamera != null)
					Player.bodyCamera.clearFlags = UnityEngine.CameraClearFlags.Skybox;
			}

			if (disableCameraOptimisations.Value)
			{
				return false;
			}
			else
				return true;
		}

		[HarmonyPrefix]
		[HarmonyPatch(typeof(DisableShadows), "Start")]
		public static bool DetourDisableShadows()
		{
			if (enableShadows.Value)
				return false;
			else
				return true;
		}
	}
}
