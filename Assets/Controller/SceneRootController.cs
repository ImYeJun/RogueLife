using System;
using UnityEngine;
namespace Controller
{
    public abstract class SceneRootController : MonoBehaviour {
        protected GameRun currentRun;

        private void Awake() {
            currentRun = GameRunManager.Instance?.CurrentRun;

            if (currentRun is null)
            {
                Debug.LogError($"[{GetType().Name}] There's no GameRunManager or GameRun.");

                return;
            }

            OnInitialize();
        }

        protected abstract void OnInitialize();
    }
}