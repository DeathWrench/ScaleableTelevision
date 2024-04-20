using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using BepInEx.Configuration;

namespace ScaleableTV
{
    [BepInPlugin("ScaleableTV", "\u200bScaleableTelevision", "1.0.8")]
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
                GameObject televisionMesh = GameObject.Find("TelevisionMesh").gameObject;
                televisionMesh.transform.localScale = new Vector3(0.01108255f * ConfigManager.tvScaleY.Value, 0.01108255f * ConfigManager.tvScaleX.Value, 0.01108255f * ConfigManager.tvScaleZ.Value);

                GameObject cube = televisionContainer.transform.Find("Cube").gameObject;

                televisionContainer.transform.GetComponentInChildren<PlaceableShipObject>().yOffset = 0.52f * (ConfigManager.tvScaleX.Value - 1) + 0.52f;
                if (!ConfigManager.configBiggerInteractRadius.Value)
                {
                    float desiredValueAt1 = 0.3115522f;
                    float desiredValueAt4096 = -1638.4f;
                    float desiredValue = Mathf.Lerp(desiredValueAt1, desiredValueAt4096, (ConfigManager.tvScaleX.Value - 1f) / 4095f);
                    cube.transform.localPosition = new Vector3(
                        desiredValue,
                        0.01403493f,
                        0.03700018f
                    );
                }
                else
                {
                    cube.transform.localPosition = new Vector3(0.3f, 0.521f, -0.3f);
                    cube.transform.localScale = new Vector3(
                        ConfigManager.tvScaleY.Value * 1.1f, 
                        ConfigManager.tvScaleX.Value * 1f, 
                        ConfigManager.tvScaleZ.Value * 1f
                    );
                }
                return;
            }
        }
    }
}
