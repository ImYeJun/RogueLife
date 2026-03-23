using UnityEngine;
using UnityEngine.EventSystems;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class GraveDeckButtonView : BattleDeckButtonView
    {
        [Header("GraveDeckButtonView Behaviour")]
        [SerializeField] private DeckInventoryView deckInventoryView;
        
        public override void OnInitialDeckSettled(InitialDeckSettled payload)
        {
            deck = payload.GraveDeck;
            DrawDeckCountText(deck.Count);
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            eventBus.Subscribe<CardDiscarded>(OnCardDiscarded);
            eventBus.Subscribe<CardRestored>(OnCardRestored);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<CardDiscarded>(OnCardDiscarded);
            eventBus?.Unsubscribe<CardRestored>(OnCardRestored);
        }

        private void OnCardDiscarded(CardDiscarded payload)
        {
            int targetCount = deck.Count;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardDiscarded_DrawGraveDeckCount, 
                DrawDeckCountTextPresentation(targetCount)
            );
        }

        private void OnCardRestored(CardRestored payload)
        {
            int targetCount = deck.Count;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardRestored_DrawGraveDeckCount, 
                DrawDeckCountTextPresentation(targetCount)
            );
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            deckInventoryView.SetActive(true);
        }
    }
}