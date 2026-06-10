using UnityEngine;

namespace InfiniteVoidRPG.UI.Hub
{
    public class UpgradeTreeScreen : Screen
    {
        private UpgradeTreeScreenView _concreteView => _view as UpgradeTreeScreenView;

        public UpgradeTreeScreen(UpgradeTreeScreenView view) : base(view)
        {
        }
    }
}
