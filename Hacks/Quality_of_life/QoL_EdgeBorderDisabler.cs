using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace AbemoreSuisHack.Hacks.QoL
{
	[HarmonyPatch]
	public class QoL_EdgeBorderDisabler : MonoBehaviour
	{
		public static ConfigEntry<bool> use;

		[HarmonyPrefix]
		[HarmonyPatch(typeof(EdgeEffect), "Start")]
		public static bool StartDetour(EdgeEffect __instance)
		{
			if (use.Value)
			{
				Destroy(__instance.gameObject);
				return false;
			}
			else
				return true;
		}
	}
}
