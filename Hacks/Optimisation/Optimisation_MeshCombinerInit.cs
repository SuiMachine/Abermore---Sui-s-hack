using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AbemoreSuisHack;
using UnityEngine.TestTools;

namespace AbemoreSuisHack.Hacks.Optimisation
{
	[HarmonyPatch]
	public class Optimisation_MeshCombinerInit : MonoBehaviour
	{
		public static ConfigEntry<bool> use;

		[HarmonyPostfix]
		[HarmonyPatch(typeof(PlayerManager), "Start")]
		public static void StartDetour(PlayerManager __instance)
		{
			if (!use.Value)
				return;

			ProcessRootObjects(__instance);
		}

		public class PairingGroup
		{
			public Vector3 avgPosition;
			public Material material;
			public List<GameObject> objectsToGroup;

			public PairingGroup()
			{
				this.avgPosition = Vector3.zero;
				this.objectsToGroup = new List<GameObject>();
			}

			public PairingGroup(GameObject obj, Material material)
			{
				this.avgPosition = obj.transform.position;
				this.material = material;
				this.objectsToGroup = new List<GameObject> { obj };
			}

			public void AddElement(GameObject obj)
			{
				objectsToGroup.Add(obj);
				var positions = objectsToGroup.Select(x => x.transform.position).ToArray();
				avgPosition = Vector3Ext.Average(positions);
			}
		}

		private static void ProcessRootObjects(PlayerManager __instance)
		{
			var scene = __instance.gameObject.scene;
			var rootObjects = scene.GetRootGameObjects().Where(x => x.activeInHierarchy && x.GetComponent<MeshFilter>() != null && x.GetComponent<Animator>() == null && x.GetComponent<Rigidbody>() == null && x.GetComponent<MeshRenderer>() != null && x.GetComponent<MeshRenderer>().enabled).ToList();

			var pairingGroups = new List<PairingGroup>();

			for (int i = rootObjects.Count - 1; i >= 0; i--)
			{
				var obj = rootObjects[i];
				var renderer = obj.GetComponent<MeshRenderer>();
				if (renderer == null || renderer.materials.Length > 1 || renderer.material == null)
					continue;
				var material = renderer.sharedMaterial;

				var pairingGroupToUse = pairingGroups.FirstOrDefault(x => Vector3.Distance(x.avgPosition, obj.transform.position) < 150 && x.material == material);
				if (pairingGroupToUse == null)
				{
					var newPairingGroup = new PairingGroup(obj, material);
					newPairingGroup.AddElement(obj);
					pairingGroups.Add(newPairingGroup);
				}
				else
				{
					pairingGroupToUse.AddElement(obj);
				}
			}

			pairingGroups = pairingGroups.Where(x => x.objectsToGroup.Count > 1).ToList();

			var originalMeshCount = 0;
			var combinedMeshCount = 0;
			try
			{
				foreach (var pairingGroup in pairingGroups)
				{
					var gos = pairingGroup.objectsToGroup;
					var resultGo = new GameObject($"Combined_{gos.First().name}");
					resultGo.transform.position = pairingGroup.avgPosition;
					resultGo.transform.rotation = Quaternion.identity;


					var meshCominers = new CombineInstance[gos.Count];

					for (int i = 0; i < gos.Count; i++)
					{
						var mf = gos[i].GetComponent<MeshFilter>();
						var mr = gos[i].GetComponent<MeshRenderer>();

						gos[i].transform.SetParent(resultGo.transform, true);
						gos[i].transform.position -= resultGo.transform.position;

						var combiner = new CombineInstance
						{
							mesh = mf.sharedMesh,
							transform = mf.transform.localToWorldMatrix
						};

						gos[i].transform.position += resultGo.transform.position;

						meshCominers[i] = combiner;
						//gos[i].SetActive(false);
						originalMeshCount++;
					}

					var resultMeshFilter = resultGo.AddComponent<MeshFilter>();
					resultMeshFilter.sharedMesh = new Mesh();
					resultMeshFilter.sharedMesh.CombineMeshes(meshCominers);
					resultMeshFilter.sharedMesh.RecalculateBounds();

					var resultMeshRenderer = resultGo.AddComponent<MeshRenderer>();
					resultMeshRenderer.sharedMaterial = pairingGroup.material;
					resultMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

					for (int i = 0; i < gos.Count; i++)
					{
						Destroy(gos[i].GetComponent<MeshFilter>());
						Destroy(gos[i].GetComponent<MeshRenderer>());
					}

					combinedMeshCount++;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
			Plugin.log.LogInfo($"Combined meshes in scene {scene.name} - original {originalMeshCount} -> reduced {combinedMeshCount}");
		}
	}
}
