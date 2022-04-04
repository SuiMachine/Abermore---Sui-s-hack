using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AbemoreSuisHack.Hacks.Optimisation
{
	[HarmonyPatch]
	public class DisableOutdoorCamera
	{
		public static ConfigEntry<bool> use;
		[HarmonyPostfix]
		[HarmonyPatch(typeof(CityPostProcessingManager), "Start")]
		public static void DetourStart(CityPostProcessingManager __instance)
		{
			if(use.Value)
				__instance.gameObject.AddComponent<DisableOutdoorCameraBehaviour>();
		}
	}

	public class DisableOutdoorCameraBehaviour : MonoBehaviour
	{
		private Camera cam;

		void LateUpdate()
		{
			if(cam == null)
			{
				cam = GetComponent<Camera>();
			}
			else
			{
				cam.enabled = false;
			}
		}
	}
}
