namespace InfiniteVoidRPG.Game.Settings
{
    public interface ISetting
    {
        public object GetValue();
        public string GetNameOfValue();

        public bool IsMinValue();
        public bool IsMaxValue();
        
        public object ToNextValue();
        public object ToPreviousValue();
        
        public void ResetToDefault();
        public void Save(System.Action<object> onSaved = null);
        public void ResetToSaved();
    }
}
