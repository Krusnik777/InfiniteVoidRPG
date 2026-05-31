namespace InfiniteVoidRPG.UI.Common
{
    public class StoryEventScreen : Screen
    {
        private StoryEventScreenView _concreteView => _view as StoryEventScreenView;

        public StoryEventScreen(StoryEventScreenView view) : base(view) {}

        public void ShowMessage(string text) // Temp
        {
            if (!_concreteView.MessagePanel.activeSelf) _concreteView.MessagePanel.SetActive(true);

            _concreteView.MessageText.text = text;
        }

        public override void Hide()
        {
            base.Hide();

            if (_concreteView.MessagePanel.activeSelf) _concreteView.MessagePanel.SetActive(false);
        }
    }
}
