using System;
using R3;

namespace InfiniteVoidRPG.Utils
{
    public static class R3Extensions
    {
        public static IDisposable DelayedCall(float delaySec, Action action)
        {
            if (delaySec <= 0)
            {
                action?.Invoke();
                return null;
            }

            return Observable.Timer(TimeSpan.FromSeconds(delaySec)).Subscribe(_ => action?.Invoke());
        }
    }
}
