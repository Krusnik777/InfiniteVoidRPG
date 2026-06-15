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
            const float eps = 0.02f;

            if (Mathf.Abs(aspect - 16f / 9f) <= eps) return "16:9";
            if (Mathf.Abs(aspect - 16f / 10f) <= eps) return "16:10";
            if (Mathf.Abs(aspect - 21f / 9f) <= eps) return "21:9";
            if (Mathf.Abs(aspect - 4f / 3f) <= eps) return "4:3";
            if (Mathf.Abs(aspect - 5f / 4f) <= eps) return "5:4";

            return $"{Mathf.Round(aspect * 100) / 100f:0.##}:1";
        }

        public override string ToString()
        {
            return $"{width}x{height} [{GetAspectRatioString()}]";
        }

    }
}
