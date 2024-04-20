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
        //public static ConfigEntry<float> tvPositionY { get; private set; }
        public static ConfigEntry<bool> configBiggerInteractRadius { get; internal set; }
        //public static ConfigEntry<bool> tvLightEnabled { get; internal set; }
        public static ConfigFile configFile { get; private set; }

        private ConfigManager(ConfigFile cfg)
        {
            configFile = cfg;

            tvScaleX = cfg.Bind("Options", "TV Scale X", 4f, "X Coordinates\n Left/Right\n Make sure you put a ''f'' at the end of the number.");
            tvScaleY = cfg.Bind("Options", "TV Scale Y", 2.2f, "Y Coordinates\n Up/Down\n Make sure you put a ''f'' at the end of the number.");
            tvScaleZ = cfg.Bind("Options", "TV Scale Z", 0.5f, "Z Coordinates\n Front/Back\n Make sure you put a ''f'' at the end of the number.");
            configBiggerInteractRadius = cfg.Bind("Accessibility", "Bigger Interact Radius", true, "You can interact with the TV by just looking at it if you're close enough, rather than only the bottom right corner.");
            //tvLightEnabled = cfg.Bind("Performance", "TV Lights", false, "Warning: If this is set to true, and TV Scale X,Y, and Z is set to anything but 1, it will break shadows and spam errors in the log");
        }
    }
}