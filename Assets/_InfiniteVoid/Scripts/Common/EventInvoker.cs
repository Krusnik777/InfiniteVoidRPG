using System;

namespace InfiniteVoidRPG
{
    public class EventInvoker
    {
        private Action _bindedAction;

        public EventInvoker(Action action)
        {
            _bindedAction = action;
        }

        public void InvokeBindedAction()
        {
            _bindedAction?.Invoke();
        }
    }
}
