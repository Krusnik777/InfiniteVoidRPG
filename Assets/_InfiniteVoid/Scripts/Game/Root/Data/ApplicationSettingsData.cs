using InfiniteVoidRPG.Game.Settings;

namespace InfiniteVoidRPG.Game.Data
{
    [System.Serializable]
    public class ApplicationSettingsData
    {
        public int CurrentLanguageIndex;
        public GraphicsData Graphics;
        public AudioData Audio;

        public ApplicationSettingsData(int currentLanguageIndex, GraphicsData graphics, AudioData audio)
        {
            CurrentLanguageIndex = currentLanguageIndex;
            Graphics = graphics;
            Audio = audio;
        }

        public ApplicationSettingsData(int currentLanguageIndex, ApplicationSettingsConfig config)
        {
            CurrentLanguageIndex = currentLanguageIndex;
            Graphics = new GraphicsData(config.DefaultResolutionIndex, (int) config.ScreenMode, config.VSyncEnabled);
            Audio = new AudioData(config.SFXVolume, config.BGMVolume);
        }
    }
}
