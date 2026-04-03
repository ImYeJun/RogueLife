using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

namespace View.BattleView
{
    public abstract class BattleDeckButtonView : ViewBehaviour<IBattleViewEvent>, IPointerClickHandler
    {
        [Header("Behaviour")]
        [SerializeField] private TextMeshProUGUI deckCountText;
        protected IReadOnlyBattleDeck deck;

        protected abstract BattleDeckType TargetDeckType { get; } 

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialDeckSettled>(OnInitialDeckSettled);
            eventBus.Subscribe<CardDiscarded>(OnCardDiscarded);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialDeckSettled>(OnInitialDeckSettled);
            eventBus?.Unsubscribe<CardDiscarded>(OnCardDiscarded);
        }

        public abstract void OnInitialDeckSettled(InitialDeckSettled payload);

        protected void DrawDeckCountText(int currentCount)
        {
            deckCountText.text = currentCount.ToString();
        }

        protected IEnumerator DrawDeckCountTextPresentation(int currentCount)
        {
            yield return null;
            DrawDeckCountText(currentCount);
        }

        protected virtual void OnCardDiscarded(CardDiscarded payload)
        {
            if (payload.Destination != TargetDeckType) { return; }

            int targetCount = deck.Count;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardDiscarded_DrawDeckCount, 
                DrawDeckCountTextPresentation(targetCount)
            );
        }

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}