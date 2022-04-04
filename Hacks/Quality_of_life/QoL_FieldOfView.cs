using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AbemoreSuisHack.Hacks.Quality_of_life
{
	[HarmonyPatch]
	public class QoL_FieldOfView
	{
		public static ConfigEntry<float> lookFOV;

		[HarmonyPrefix]
		[HarmonyPatch(typeof(FPSView), "Start")]
		public static void StartDetour(FPSView __instance)
		{
			if (lookFOV.Value > 50 && lookFOV.Value < 100)
			{
				var expension = lookFOV.Value - 65f;
				__instance.grabFOV = expension;
			}
		}
	}
}
