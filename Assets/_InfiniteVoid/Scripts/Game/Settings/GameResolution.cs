using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    [System.Serializable]
    public class GameResolution
    {
        public int width;
        public int height;

        public GameResolution(int w, int h)
        {
            width = w;
            height = h;
        }

        public string GetAspectRatioString()
        {
            float aspect = (float)width / height;
            
            if (Mathf.Approximately(aspect, 16f / 9f)) return "16:9";
            if (Mathf.Approximately(aspect, 16f / 10f)) return "16:10";
            if (Mathf.Approximately(aspect, 21f / 9f)) return "21:9";
            if (Mathf.Approximately(aspect, 4f / 3f)) return "4:3";
            if (Mathf.Approximately(aspect, 5f / 4f)) return "5:4";
            
            return $"{Mathf.Round(aspect * 100) / 100}:1";
        }

        public override string ToString()
        {
            return $"{width}x{height} [{GetAspectRatioString()}]";
        }

    }
}
