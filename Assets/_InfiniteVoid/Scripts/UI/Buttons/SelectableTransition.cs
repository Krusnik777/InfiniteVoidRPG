using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [System.Serializable]
    public class SelectableTransition
    {
        public enum SelectType
        {
            None,
            Indicator,
            Color,
            Animation
        }

        public SelectType Type;
        public GameObject SelectedIndicator;
        public Graphic TargetGraphic;
        public Color SelectedColor;
        public Color UnselectedColor;
        public float TargetScale = 1.2f;
        public float AnimationTime = 0.25f;
    }
}
