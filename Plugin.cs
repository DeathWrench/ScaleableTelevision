using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using UnityEngine.UIElements;

namespace DeathWrench.ScaleableTelevision
{
    [BepInPlugin($"{PLUGIN_GUID}", $"{PLUGIN_NAME}", $"{PLUGIN_VERSION}")]
    [HarmonyPatch]
    public class ScaleableTelevision : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "DeathWrench.ScaleableTelevision";
        public const string PLUGIN_NAME = "\u200bScaleableTelevision";
        public const string PLUGIN_VERSION = "1.0.10";
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
                GameObject televisionMesh = GameObject.Find("TelevisionMesh").gameObject;
                televisionMesh.transform.localScale = new Vector3(0.01108255f * ConfigManager.tvScaleY.Value, 0.01108255f * ConfigManager.tvScaleX.Value, 0.01108255f * ConfigManager.tvScaleZ.Value);

                GameObject cube = televisionContainer.transform.Find("Cube").gameObject;

                televisionContainer.transform.GetComponentInChildren<PlaceableShipObject>().yOffset = 0.52f * (ConfigManager.tvScaleY.Value - 1);
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
                    cube.transform.localScale = new Vector3(
                        ConfigManager.tvScaleY.Value * 0.2601791f,
                        ConfigManager.tvScaleX.Value * 0.405167f,
                        ConfigManager.tvScaleZ.Value * 0.1014986f
                    );
                }
                else
                {
                    float desiredPosXValueAt1 = 0.521f;
                    float desiredPosYValueAt1 = 0.3f;
                    //float desiredPosZValueAt1 = -0.3f;
                    float desiredPosXValueAt4096 = -1625.286f;
                    float desiredPosYValueAt4096 = 350.3f;
                    //float desiredPosZValueAt4096 = -2.9475f;
                    float desiredPosX = Mathf.Lerp(desiredPosXValueAt1, desiredPosXValueAt4096, (ConfigManager.tvScaleY.Value - 1f) / 4095f);
                    float desiredPosY = Mathf.Lerp(desiredPosYValueAt1, desiredPosYValueAt4096, (ConfigManager.tvScaleX.Value - 1f) / 4095f);
                    //float desiredPosZ = Mathf.Lerp(desiredPosZValueAt1, desiredPosZValueAt4096, (ConfigManager.tvScaleX.Value - 1f) / 4095f);
                    cube.transform.localPosition = new Vector3(desiredPosX, desiredPosY, cube.transform.localPosition.z);
                    cube.transform.localScale = new Vector3(
                        ConfigManager.tvScaleY.Value * 1f, 
                        ConfigManager.tvScaleX.Value * 1.1f,
                        1f
                    );
                }
                return;
            }
        }
    }
}
