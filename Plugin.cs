using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using BepInEx.Configuration;

namespace ScaleableTV
{
    [BepInPlugin("ScaleableTV", "ScaleableTelevision", "1.0.6")]
    [HarmonyPatch]
    public class ScaleableTelevision : BaseUnityPlugin
    {
        static new GameObject? gameObject;
        void Awake()
        {
            ConfigManager.Init(Config);
            Logger.LogInfo($"Plugin {"ScaleableTelevision"} is loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
        public static void SetTVScreenMaterial(TVScript __instance, bool b)
        {
            MethodInfo method = __instance.GetType().GetMethod("SetTVScreenMaterial", BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(__instance, new object[]
            {
        b
            });
            if (!ConfigManager.tvLightEnabled.Value)
            __instance.tvLight.enabled = false;
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
                if (!ConfigManager.tvLightEnabled.Value)
                {
                    GameObject TelevisionContainer = __instance.gameObject.transform.parent.gameObject;
                    TelevisionContainer.transform.localScale = new Vector3(ConfigManager.tvScaleX.Value, ConfigManager.tvScaleY.Value, ConfigManager.tvScaleZ.Value);
                }
                if (!ConfigManager.configBiggerInteractRadius.Value)
                { return; }
                else 
                { 
                     GameObject Cube = __instance.gameObject.transform.gameObject;
                     Cube.transform.localPosition = new Vector3(0.521f, 0.3f, -0.3f);
                     Cube.transform.localScale = new Vector3(1f, 1.1f, 1f);
                } return;
            }
        }
        public Light tvLight;
    }
}
