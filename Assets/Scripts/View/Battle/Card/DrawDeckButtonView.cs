using UnityEngine;
using UnityEngine.EventSystems;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class DrawDeckButtonView : BattleDeckButtonView
    { 
        [Header("DrawDeckButtonView Behaviour")]
        [SerializeField] private DeckInventoryView deckInventoryView;

        public override void OnInitialDeckSettled(InitialDeckSettled payload)
        {
            deck = payload.DrawDeck;
            DrawDeckCountText(deck.Count);
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            eventBus.Subscribe<CardDrawed>(OnCardDrawed);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            eventBus?.Unsubscribe<CardDrawed>(OnCardDrawed);
        }
        private void OnCardDrawed(CardDrawed payload)
        {
            int targetCount = deck.Count; 
            
            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardDrawed_DrawDrawDeckCount, 
                DrawDeckCountTextPresentation(targetCount)
            );
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            deckInventoryView.SetActive(true);
        }
    }
}