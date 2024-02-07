using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using BepInEx.Configuration;

namespace ScaleableTV
{
    [BepInPlugin("ScaleableTV", "ScaleableTelevision", "1.0.0")]
    [HarmonyPatch]
    public class ScaleableTelevision : BaseUnityPlugin
    {
        static new GameObject gameObject;
        static GameObject tvMesh = null;
        void Awake()
        {
            ConfigManager.Init(Config);
            Logger.LogInfo($"Plugin {"ScaleableTelevision"} is loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
        void Update()
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("InteractTrigger");
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].name == "Cube" && array[i].transform.parent.gameObject.name.StartsWith("TelevisionContainer"))
                {
                    Patchez.PostFix_adjustTVScale(array[i].gameObject);
                }
            }
        }
        public static class Patchez
        {
            [HarmonyPatch(typeof(GameObject), "TelevisionContainer")]
            [HarmonyPostfix]
            public static void PostFix_adjustTVScale(GameObject __instance)
            {
                tvMesh = __instance;
                GameObject TelevisionContainer = __instance.gameObject.transform.parent.gameObject;
                TelevisionContainer.transform.localScale = new Vector3(ConfigManager.tvScaleX.Value, ConfigManager.tvScaleY.Value, ConfigManager.tvScaleZ.Value);
            }
        }
    }
}