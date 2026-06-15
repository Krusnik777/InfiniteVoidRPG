using UnityEngine;

namespace InfiniteVoidRPG.UI
{
    public class ConfirmWindow : Popup
    {
        public override bool IsMultipleInstancesAllowed => true;

        private ConfirmWindowView _concreteView => _view as ConfirmWindowView;

        public ConfirmWindow(ConfirmWindowView view) : base(view) { }

        public override void Initialize(IPopupInitData initData = null)
        {
            
        }
    }
}
