namespace InfiniteVoidRPG.Game.Settings
{
    public interface ISetting
    {
        public object GetValue();
        public string GetNameOfValue();
        public bool IsCurrentValueApplied();
        public float GetCurrentValueDifference();

        public bool IsMinValue();
        public bool IsMaxValue();
        
        public object ToNextValue(bool applyChanges = true);
        public object ToPreviousValue(bool applyChanges = true);
        
        public void Apply();
        public void ResetToDefault();
    }
}
