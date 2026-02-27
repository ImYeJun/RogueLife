using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Controller
{
    public abstract class SceneRootController : MonoBehaviour {
        protected GameRun currentRun;

        private void Awake()
        {
#if UNITY_EDITOR
            if (GameRunManager.Instance is null)
            {
                StartCoroutine(AwakeWithCoreSceneLoad());

                return;
            }
#endif
            OnAwake();
        }

#if UNITY_EDITOR
        private IEnumerator AwakeWithCoreSceneLoad()
        {
            EnsureCoreSceneLoaded();

            yield return new WaitUntil(() =>
            {
                Scene coreScene = SceneManager.GetSceneByName("Core");

                return coreScene.isLoaded;
            });

            GameSceneManager.Instance.SetCurrentScene((SceneName)SceneManager.GetActiveScene().buildIndex);
            GameRunManager.Instance.StartNewRun();
            OnAwake();
        }

        private void EnsureCoreSceneLoaded()
        {
            Scene coreScene = SceneManager.GetSceneByName("Core");

            if (coreScene.isLoaded) { return; }

            SceneManager.LoadScene("Core", LoadSceneMode.Additive);
        }
#endif

        private void OnAwake()
        {
            currentRun = GameRunManager.Instance?.CurrentRun;

            if (currentRun is null)
            {
                Debug.LogError("[SceneRootController] There's no GameRunManager or GameRun.");

                return;
            }

        }
        
        public void Start(){
            OnInitialize();
        }

        protected abstract void OnInitialize();
    }
}