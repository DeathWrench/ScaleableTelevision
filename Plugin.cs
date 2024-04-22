using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

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
                GameObject placementCollider = televisionContainer.transform.Find("PlacementCollider").gameObject;
                BoxCollider collider = placementCollider.GetComponent<BoxCollider>();
                collider.size = new Vector3(ConfigManager.tvScaleY.Value, ConfigManager.tvScaleX.Value, 0.7f);

                float centerXAtScale1 = 0f;
                float centerYAtScale1 = 0f;
                float centerXAtScale8192 = 1536f;
                float centerYAtScale8192 = ConfigManager.tvScaleY.Value / 2.9333f;

                float desiredCenterX = Mathf.Lerp(centerXAtScale1, centerXAtScale8192, (ConfigManager.tvScaleX.Value - 1f) / 8191f);
                float desiredCenterY = Mathf.Lerp(centerYAtScale1, centerYAtScale8192, (ConfigManager.tvScaleY.Value - 1f) / 8191f); // Use the adjusted ratio

                collider.center = new Vector3(desiredCenterX, desiredCenterY, 0.7f);

                if (!ConfigManager.configBiggerInteractRadius.Value)
                {
                    float desiredValueAt1 = 0.3115522f;
                    float desiredValueAt8 = -3.25f;
                    float desiredValueAt4096 = -1662.5f;
                    float desiredValue;
                    float t;
                    if (ConfigManager.tvScaleX.Value <= 8)
                    {
                        t = (ConfigManager.tvScaleX.Value - 1f) / 7f;
                        desiredValue = Mathf.Lerp(desiredValueAt1, desiredValueAt8, t);
                    }
                    else
                    {
                        t = Mathf.Lerp(8f, 4096f, (ConfigManager.tvScaleX.Value - 8f) / 4088f);
                        desiredValue = Mathf.Lerp(desiredValueAt8, desiredValueAt4096, t);
                    }
                    cube.transform.localPosition = new Vector3(
                        desiredValue,
                        0.01403493f,
                        0.03700018f
                    );
                    cube.transform.localScale = new Vector3(
                        ConfigManager.tvScaleY.Value * 0.2601791f,
                        ConfigManager.tvScaleY.Value * 0.405167f,
                        0.1014986f
                    );
                }
                else
                {
                    float desiredPosXValueAt1 = 0.521f;
                    float desiredPosYValueAt1 = 0.3f;
                    float desiredPosXValueAt4096 = -1625.286f;
                    float desiredPosYValueAt4096 = 350.3f;
                    float desiredPosX = Mathf.Lerp(desiredPosXValueAt1, desiredPosXValueAt4096, (ConfigManager.tvScaleY.Value - 1f) / 4095f);
                    float desiredPosY = Mathf.Lerp(desiredPosYValueAt1, desiredPosYValueAt4096, (ConfigManager.tvScaleX.Value - 1f) / 4095f);
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
