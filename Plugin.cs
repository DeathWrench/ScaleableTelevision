using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace DeathWrench.ScaleableTelevision
{
    [BepInPlugin($"{PLUGIN_GUID}", $"{PLUGIN_NAME}", $"{PLUGIN_VERSION}")]
    public class ScaleableTelevision : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "DeathWrench.ScaleableTelevision";
        public const string PLUGIN_NAME = "\u200bScaleableTelevision";
        public const string PLUGIN_VERSION = "2.0.1";
        static new GameObject? gameObject;
        void Awake()
        {
            ConfigManager.Init(Config);
            Logger.LogInfo($"Plugin {PLUGIN_NAME} v{PLUGIN_VERSION} is loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
        void Update()
        {
            GameObject[] interactTriggers = GameObject.FindGameObjectsWithTag("InteractTrigger");
            foreach (GameObject trigger in interactTriggers)
            {
                if (trigger.name == "Cube" && trigger.transform.parent.gameObject.name.StartsWith("TelevisionContainer"))
                {
                    GameObject televisionContainer = trigger.transform.parent.gameObject;
                    GameObject televisionMesh = televisionContainer.transform.Find("TelevisionMesh").gameObject;
                    GameObject tvAudio = televisionContainer.transform.Find("TVAudio").gameObject;
                    GameObject cube = televisionContainer.transform.Find("Cube").gameObject;

                    AudioSource audioSource = tvAudio.GetComponent<AudioSource>();
                        
                    if (televisionMesh.transform.localScale != new Vector3(0.01108255f * ConfigManager.tvScaleY.Value, 0.01108255f * ConfigManager.tvScaleX.Value, 0.01108255f * ConfigManager.tvScaleZ.Value) ||
                        audioSource.minDistance != ConfigManager.audioSourceMinDistance.Value ||
                        audioSource.maxDistance != ConfigManager.audioSourceMaxDistance.Value ||
                        (cube.transform.localScale != new Vector3(ConfigManager.tvScaleY.Value * 1f, ConfigManager.tvScaleX.Value * 1.1f, 1f) && ConfigManager.configBiggerInteractRadius.Value) ||
                        (cube.transform.localScale != new Vector3(ConfigManager.tvScaleY.Value * 0.2601791f, ConfigManager.tvScaleY.Value * 0.405167f, ConfigManager.tvScaleZ.Value * 0.1014986f) && !ConfigManager.configBiggerInteractRadius.Value))
                    {
                        Patchez.PostFix_adjustTVScale(trigger);
                    }
                    else return;
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
                BoxCollider placementBoxCollider = placementCollider.GetComponent<BoxCollider>();
                placementBoxCollider.size = new Vector3(ConfigManager.tvScaleY.Value, ConfigManager.tvScaleX.Value, 0.7f);

                float placementBoxColliderDesiderCenterX = Mathf.Lerp(0f, 1536f, (ConfigManager.tvScaleX.Value - 1f) / 8191f);
                float placementBoxColliderDesiderCenterY = Mathf.Lerp(0f, ConfigManager.tvScaleY.Value / 2.9333f, (ConfigManager.tvScaleY.Value - 1f) / 8191f);


                GameObject tvAudio = televisionContainer.transform.Find("TVAudio").gameObject;
                tvAudio.transform.localPosition = new Vector3(-ConfigManager.tvScaleY.Value * 0.39f, ConfigManager.tvScaleY.Value * 0.44f, -0.42f);

                AudioSource audioSource = tvAudio.GetComponent<AudioSource>();
                //audioSource.spatialBlend = 1.0f;
                audioSource.minDistance = ConfigManager.audioSourceMinDistance.Value;
                audioSource.maxDistance = ConfigManager.audioSourceMaxDistance.Value;

                placementBoxCollider.center = new Vector3(placementBoxColliderDesiderCenterX, placementBoxColliderDesiderCenterY, 0.7f);

                if (!ConfigManager.configBiggerInteractRadius.Value)
                {
                    float desiredValue;
                    float t;
                    if (ConfigManager.tvScaleX.Value <= 8)
                    {
                        t = (ConfigManager.tvScaleX.Value - 1f) / 7f;
                        desiredValue = Mathf.Lerp(0.3115522f, -3.25f, t);
                    }
                    else
                    {
                        t = Mathf.Lerp(8f, 4096f, (ConfigManager.tvScaleX.Value - 8f) / 4088f);
                        desiredValue = Mathf.Lerp(-3.25f, -1662.5f, t);
                    }
                    cube.transform.localPosition = new Vector3(
                        desiredValue,
                        0.01403493f,
                        0.03700018f
                    );
                    cube.transform.localScale = new Vector3(
                        ConfigManager.tvScaleY.Value * 0.2601791f,
                        ConfigManager.tvScaleY.Value * 0.405167f,
                        ConfigManager.tvScaleZ.Value * 0.1014986f
                    );
                }
                else
                {
                    float desiredPosX = Mathf.Lerp(0.521f, -1625.286f, (ConfigManager.tvScaleY.Value - 1f) / 4095f);
                    float desiredPosY = Mathf.Lerp(0.3f, 350.3f, (ConfigManager.tvScaleX.Value - 1f) / 4095f);
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
