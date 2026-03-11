using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using TMPro;

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
            DrawDeckCountText();
        }

        private void OnCardDiscarded(CardDiscarded payload)
        {
            DrawDeckCountText();
        }

        private void OnCardRestored(CardRestored payload)
        {
            DrawDeckCountText();
        }

        private void DrawDeckCountText()
        {
            if (handDeck == null || graveDeck == null) return;

            drawDeckCountText.text = handDeck.Count.ToString();
            graveDeckCountText.text = graveDeck.Count.ToString();
        }
    }
}