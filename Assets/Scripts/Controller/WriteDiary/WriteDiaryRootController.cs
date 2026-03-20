
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.Core;
using ViewEvent.WriteDiaryView;

namespace Controller.WriteDiary
{
    public class WriteDiaryRootController : InGameRootController
    {
        private WriteDiaryViewEventBus viewEventBus;
        private IWriteDiaryViewCommander viewCommander;

        [SerializeField] private List<ViewBehaviour<IWriteDiaryViewEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IWriteDiaryViewEvent, IWriteDiaryViewCommander>> interacatbleViews;

        protected override void OnInitialize()
        {
            viewCommander = currentRun.WriteDiaryViewCommander;
            viewEventBus = currentRun.WriteDiaryViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(random, viewEventBus, PresentationManager.Instance);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(random, viewEventBus, PresentationManager.Instance ,viewCommander);
            }

            viewEventBus.Subscribe<ReturnToMainMenuRequested>(OnReturnToMainMenuRequested);

            viewCommander.WriteDiary();
        }

        private void OnDestroy() {
            viewEventBus?.Unsubscribe<ReturnToMainMenuRequested>(OnReturnToMainMenuRequested);
        }

        private void OnReturnToMainMenuRequested(ReturnToMainMenuRequested payload){
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.ReturnToMainMenuRequested_SceneTransition, TransitionToMainMenu());
        }
        private IEnumerator TransitionToMainMenu()
        {
            yield return null;
            GameSceneManager.Instance.LoadScene(SceneName.MAIN_MENU);
        }
    }
}