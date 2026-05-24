using DI;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.EntryPoints
{
    public class GameplayEntryPoint : EntryPoint
    {
        private Subject<string> _onEnd;

        public override Observable<string> Run(DIContainer sceneContainer)
        {
            Debug.Log("ENTRY POINT: Started Gameplay");

            _onEnd = new();

            return _onEnd;
        }

        private void OnDestroy()
        {
            DisposeOfListeners();
        }

        private void FinishGame()
        {
            DisposeOfListeners();

            _onEnd.OnNext("FINISH");
        }

        private void DisposeOfListeners()
        {
            
        }
    }
}
