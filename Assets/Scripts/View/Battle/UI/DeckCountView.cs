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
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialDeckSettled>(OnInitialDeckSettled);
        }

        public void OnInitialDeckSettled(InitialDeckSettled payload)
        {
            handDeck = payload.HandDeck;
            graveDeck = payload.GraveDeck;

            DrawDeckCountText();
        }

        private void DrawDeckCountText()
        {
            drawDeckCountText.text = handDeck.Count.ToString();
            graveDeckCountText.text = graveDeck.Count.ToString();
        }
    }
}
