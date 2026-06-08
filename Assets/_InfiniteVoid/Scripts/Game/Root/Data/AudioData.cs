using UnityEngine;

namespace InfiniteVoidRPG.Game.Data
{
    [System.Serializable]
    public class AudioData
    {
        public int SFXVolume;
        public int BGMVolume;

        public AudioData(int sfxVolume, int bgmVolume)
        {
            SFXVolume = sfxVolume;
            BGMVolume = bgmVolume;
        }
    }
}
