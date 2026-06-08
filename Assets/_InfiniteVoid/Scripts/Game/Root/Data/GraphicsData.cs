namespace InfiniteVoidRPG.Game.Data
{
    [System.Serializable]
    public class GraphicsData
    {
        public int ResolutionIndex;
        public int ScreenModeIndex;
        public bool VSyncState;

        public GraphicsData(int resolutionIndex, int screenModeIndex, bool vSyncState)
        {
            ResolutionIndex = resolutionIndex;
            ScreenModeIndex = screenModeIndex;
            VSyncState = vSyncState;
        }
    }
}
