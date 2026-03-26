using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.StartMenu;

namespace View.StartMenu
{
    public class StartDeckSelectView : InteractableViewBehaviour<IStartMenuViewEvent, IStartMenuViewCommander>
    {
        [Header("Behaviour")]
        [SerializeField] private GameObject startDeckSelectView;
        [SerializeField] private Transform startDeckSelectItemsContainer;
        [SerializeField] private GameObject startDeckSelectButtonPrefab;

        [Header("Presentation")]
        [SerializeField] private CanvasGroup viewCanvasGroup;
        [SerializeField] private float viewAppearDuration;
        private Tween currentTween;

        public override void OnInitialized()
        {
            startDeckSelectView.SetActive(false);

            eventBus.Subscribe<StartDeckLoaded>(OnStartDeckLoaded);
            eventBus.Subscribe<ReadyToStartGame>(OnReadyToStartGame);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<StartDeckLoaded>(OnStartDeckLoaded);
            eventBus?.Unsubscribe<ReadyToStartGame>(OnReadyToStartGame);
        }

        public void OnDeckSelected(StartDeck startDeck)
        {
            commander.FixStartDeck(startDeck);
        }

        private void OnStartDeckLoaded(StartDeckLoaded payload)
        {
            for (int i = 0; i < payload.StartDecks.Count; i++)
            {
                var startDeck = payload.StartDecks[i];
                startDeckSelectView.SetActive(true);

                var button = Instantiate(startDeckSelectButtonPrefab, startDeckSelectItemsContainer);
                
                var startDeckSelectButton = button.GetComponent<StartDeckSelectButton>();

                startDeckSelectButton.Initialize(payload.SequenceId, presentationManager, i, startDeck, () => OnDeckSelected(startDeck));
            }

            viewCanvasGroup.alpha = 0;
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.StartDeckLoaded_ViewAppear, ViewAppearPresentation());
        }

        public void OnReadyToStartGame(ReadyToStartGame game)
        {
            startDeckSelectView.SetActive(false);
        }

        public IEnumerator ViewAppearPresentation()
        {
            currentTween?.Kill();

            viewCanvasGroup.alpha = 0;
            currentTween = viewCanvasGroup.DOFade(1.0f, viewAppearDuration);
            yield return currentTween.WaitForCompletion();
        }

#if UNITY_EDITOR

        [ContextMenu("Test: Open view with 3 dummy start decks")]
        public void TestFullPresentation()
        {
            foreach (Transform child in startDeckSelectItemsContainer)
            {
                Destroy(child.gameObject); 
            }

            var dummyDecks = new List<StartDeck>
            {
                ScriptableObject.CreateInstance<StartDeck>(),
                ScriptableObject.CreateInstance<StartDeck>(),
                ScriptableObject.CreateInstance<StartDeck>()
            };

            startDeckSelectView.SetActive(true);

            for (int i = 0; i < dummyDecks.Count; i++)
            {
                var startDeck = dummyDecks[i];
                var button = Instantiate(startDeckSelectButtonPrefab, startDeckSelectItemsContainer);
                var startDeckSelectButton = button.GetComponent<StartDeckSelectButton>();

                startDeckSelectButton.Initialize(0, presentationManager, i, startDeck, () => Debug.Log($"[Test] Dummy Deck {i} Selected!"));
            }

            presentationManager.Enqueue(0, PresentationPriority.StartDeckLoaded_ViewAppear, ViewAppearPresentation());
        }
#endif
    }
}