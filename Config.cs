using BepInEx.Configuration;

namespace ScaleableTV
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
        public static ConfigFile configFile { get; private set; }

        private ConfigManager(ConfigFile cfg)
        {
            configFile = cfg;

            tvScaleX = cfg.Bind("Options", "TV Scale X", 1f, "X Coordinates\n Left/Right\nHow wide");
            tvScaleY = cfg.Bind("Options", "TV Scale Y", 1f, "Y Coordinates\n Up/Down\nHow tall");
            tvScaleZ = cfg.Bind("Options", "TV Scale Z", 1f, "Z Coordinates\n Front/Back\nHow long");
            configBiggerInteractRadius = cfg.Bind("Accessibility", "Bigger Interact Radius", true, "You can interact with the TV by just looking at it if you're close enough, rather than only the bottom right corner.");
        }
    }
}