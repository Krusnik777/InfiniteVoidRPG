using System.Collections.Generic;

namespace InfiniteVoidRPG.Game.Settings
{
    public interface IGraphicsSettingsConfig
    {
        public IReadOnlyList<GameResolution> Resolutions { get; }
        public int DefaultResolutionIndex  { get; }
        public ApplicationScreenMode ScreenMode { get; }
        public bool VSyncEnabled { get; }
    }
}
