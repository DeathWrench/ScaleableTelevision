using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace ScaleableTV
{
    [BepInPlugin($"{PLUGIN_GUID}", $"{PLUGIN_NAME}", $"{PLUGIN_VERSION}")]
    [HarmonyPatch]
    public class ScaleableTelevision : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "DeathWrench.ScaleableTelevision";
        public const string PLUGIN_NAME = "\u200bScaleableTelevision";
        public const string PLUGIN_VERSION = "v1.1.0";
        static new GameObject? gameObject;
        void Awake()
        {
            ConfigManager.Init(Config);
            Logger.LogInfo($"Plugin {PLUGIN_NAME} {PLUGIN_VERSION} is loaded!");
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
                GameObject televisionContainer = __instance.gameObject.transform.parent.gameObject;
                GameObject cube = televisionContainer.transform.Find("Cube").gameObject;
                televisionContainer.transform.localScale = new Vector3(ConfigManager.tvScaleX.Value, ConfigManager.tvScaleY.Value, ConfigManager.tvScaleZ.Value);
                televisionContainer.transform.GetComponentInChildren<PlaceableShipObject>().yOffset = 0.52f * (ConfigManager.tvScaleY.Value - 1) + 0.52f;
                // doesn't work televisionContainer.transform.GetComponentInChildren<PlaceableShipObject>().transform.localScale = new Vector3(ConfigManager.tvScaleX.Value, ConfigManager.tvScaleY.Value, ConfigManager.tvScaleZ.Value);

                if (!ConfigManager.configBiggerInteractRadius.Value)
                {
                    cube.transform.localPosition = new Vector3(
                        0.3115522f,
                        0.01403493f,
                        0.03700018f
                    );
                    cube.transform.localScale = new Vector3(
                        0.2601791f,
                        0.405167f,
                        0.1014986f
                    );
                }
                else 
                { 
                     cube.transform.localPosition = new Vector3(
                         0.521f, 
                         0.3f, 
                         -0.3f);
                     cube.transform.localScale = new Vector3(
                         1f, 
                         1.1f, 
                         1f);
                } 
                return;
            }
        }
    }
}
