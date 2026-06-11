using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InfiniteVoidRPG.Game.Settings
{
    public abstract class UISetting : MonoBehaviour, System.IDisposable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject m_selectedIndicator;

        public Subject<UISetting> OnSelect { get; private set; } = new();

        protected ISetting _setting;

        public virtual void Setup(ISetting setting)
        {
            _setting = setting;
        }

        public virtual void Dispose() { }

        public virtual void OnPressed() { }

        public virtual void ChangeToNextValue() { }
        public virtual void ChangeToPreviousValue() { }

        public virtual void SetSelected(bool state)
        {
            m_selectedIndicator.gameObject.SetActive(state);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSelect?.OnNext(this);

            if (_setting == null) SetSelected(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_setting == null) SetSelected(false);
        }
    }
}
