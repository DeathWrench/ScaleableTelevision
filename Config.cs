using BepInEx.Configuration;

namespace DeathWrench.ScaleableTelevision
{
    public class ConfigManager
    {
        public static ConfigManager Instance { get; private set; }

        public static void Init(ConfigFile config)
        {
            Instance = new ConfigManager(config);
        }

        public static ConfigEntry<float> tvScaleX { get; private set; }
        public static ConfigEntry<float> tvScaleY { get; private set; }
        public static ConfigEntry<float> tvScaleZ { get; private set; }
        public static ConfigEntry<bool> configBiggerInteractRadius { get; internal set; }
        public static ConfigEntry<float> audioSourceMinDistance { get; private set; }
        public static ConfigEntry<float> audioSourceMaxDistance { get; private set; }
        public static ConfigFile configFile { get; private set; }

        private ConfigManager(ConfigFile cfg)
        {
            configFile = cfg;

            tvScaleX = cfg.Bind("Options", "TV Scale X", 1f, "X Coordinates\n Left/Right\n Make sure you put a ''f'' at the end of the number.");
            tvScaleY = cfg.Bind("Options", "TV Scale Y", 1f, "Y Coordinates\n Up/Down\n Make sure you put a ''f'' at the end of the number.");
            tvScaleZ = cfg.Bind("Options", "TV Scale Z", 1f, "Z Coordinates\n Front/Back\n Make sure you put a ''f'' at the end of the number.");
            audioSourceMinDistance = cfg.Bind("Options", "Audio Falloff Min Distance", 2f, "How far before TV audio starts to fall off? Set this value higher if the TV is huge and you can't hear it unless you're really close.");
            audioSourceMaxDistance = cfg.Bind("Options", "Audio Falloff Max Distance", 24f, "How far before you can't hear the TV anymore. Should probably keep this value higher than the minimum distance.");
            configBiggerInteractRadius = cfg.Bind("Accessibility", "Bigger Interact Radius", true, "You can interact with the TV by just looking at it if you're close enough, rather than only the bottom right corner.");
        }
    }
}