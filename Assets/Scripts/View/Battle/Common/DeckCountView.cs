using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using TMPro;
using System.Collections;

namespace View.BattleView
{
    public class DeckCountView : ViewBehaviour<IBattleViewEvent>
    {
        [SerializeField] private TextMeshProUGUI drawDeckCountText;
        [SerializeField] private TextMeshProUGUI graveDeckCountText;

        private IHandDeckContext handDeck; 
        private IGraveDeckContext graveDeck;

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialDeckSettled>(OnInitialDeckSettled);
            
            eventBus.Subscribe<CardDrawed>(OnCardDrawed);
            eventBus.Subscribe<CardDiscarded>(OnCardDiscarded);
            eventBus.Subscribe<CardRestored>(OnCardRestored);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialDeckSettled>(OnInitialDeckSettled);
            eventBus?.Unsubscribe<CardDrawed>(OnCardDrawed);
            eventBus?.Unsubscribe<CardDiscarded>(OnCardDiscarded);
            eventBus?.Unsubscribe<CardRestored>(OnCardRestored);
        }

        public void OnInitialDeckSettled(InitialDeckSettled payload)
        {
            handDeck = payload.HandDeck;
            graveDeck = payload.GraveDeck;

            DrawDeckCountText();
        }

        private void OnCardDrawed(CardDrawed payload)
        {
            int targetCount = handDeck.Count; 
            
            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardDrawed_DrawDrawDeckCount, 
                UpdateDrawDeckCountPresentation(targetCount)
            );
        }

        private void OnCardDiscarded(CardDiscarded payload)
        {
            int targetCount = graveDeck.Count;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardDiscarded_DrawGraveDeckCount, 
                UpdateGraveDeckCountPresentation(targetCount)
            );
        }

        private void OnCardRestored(CardRestored payload)
        {
            int targetGraveCount = graveDeck.Count;
            int targetDrawCount = handDeck.Count;

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardRestored_DrawGraveDeckCount, 
                UpdateGraveDeckCountPresentation(targetGraveCount)
            );

            presentationManager.Enqueue(
                payload.SequenceId, 
                PresentationPriority.CardRestored_DrawDrawDeckCount, 
                UpdateDrawDeckCountPresentation(targetDrawCount)
            );
        }

        private IEnumerator UpdateDrawDeckCountPresentation(int targetCount)
        {
            DrawDrawDeckCountText(targetCount);
            yield return null;
        }

        private IEnumerator UpdateGraveDeckCountPresentation(int targetCount)
        {
            DrawGraveDeckCountText(targetCount);
            yield return null;
        }

        private void DrawDrawDeckCountText(int currentDrawDeckCount)
        {
            drawDeckCountText.text = currentDrawDeckCount.ToString();
        }
        
        private void DrawGraveDeckCountText(int currentGraveDeckCount)
        {
            graveDeckCountText.text = currentGraveDeckCount.ToString();
        }

        private void DrawDeckCountText()
        {
            if (handDeck == null || graveDeck == null) return;

            DrawDrawDeckCountText(handDeck.Count);
            DrawGraveDeckCountText(graveDeck.Count);
        }
    }
}