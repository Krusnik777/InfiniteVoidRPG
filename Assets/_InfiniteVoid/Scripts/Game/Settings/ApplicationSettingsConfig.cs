using System.Collections.Generic;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    [CreateAssetMenu(fileName = "ApplicationSettingsConfig", menuName = "Scriptable Objects/Application Settings Config")]
    public class ApplicationSettingsConfig : ScriptableObject, IGraphicsSettingsConfig, IAudioSettingsConfig
    {
        [field: Header("Graphics")]
        [SerializeField] private List<GameResolution> _resolutions = new List<GameResolution>()
        {
            new GameResolution(1280, 720),
            new GameResolution(1366, 768),
            new GameResolution(1600, 900),
            new GameResolution(1920, 1080), // index 3
            new GameResolution(2560, 1440),
            new GameResolution(3840, 2160),
            new GameResolution(1280, 800),
            new GameResolution(1440, 900),
            new GameResolution(1680, 1050),
            new GameResolution(1920, 1200),
            new GameResolution(2560, 1600),
            new GameResolution(3840, 2400)
        };
        [field: SerializeField] public int DefaultResolutionIndex { get; private set; } = 3; // 1920 x 1080
        [field: SerializeField] public ApplicationScreenMode ScreenMode { get; private set; } = ApplicationScreenMode.FullScreenWindow;
        [field: SerializeField] public bool VSyncEnabled { get; private set; } = true;
        [field: Header("Sounds")]
        [field: SerializeField][field: Range(-80, 20)] public int SFXVolume { get; private set; } = 0;
        [field: SerializeField][field: Range(-80, 20)] public int BGMVolume { get; private set; } = 0;

        public IReadOnlyList<GameResolution> Resolutions => _resolutions;
        
        #if UNITY_EDITOR

        [ContextMenu("Sort Resolutions")]
        private void SortByArea()
        {
            _resolutions.Sort((a, b) => (a.width * a.height).CompareTo(b.width * b.height));
        }

        [ContextMenu("Clear Duplicates")]
        private void ClearDuplicates()
        {
            var unique = new HashSet<(int, int)>();
            _resolutions.RemoveAll(r => !unique.Add((r.width, r.height)));
        }

        #endif
    }
}
