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

        public static ConfigFile configFile { get; private set; }

        private ConfigManager(ConfigFile cfg)
        {
            configFile = cfg;

            tvScaleX = cfg.Bind("TVScale", "TVScaleX", 1.9f, "X Coordinate Left/Right");
            tvScaleY = cfg.Bind("TVScale", "TVScaleY", 1.6f, "Y Coordinate Up/Down");
            tvScaleZ = cfg.Bind("TVScale", "TVScaleZ", 0.5f, "Z Coordinate Front/Back");
        }
    }
}