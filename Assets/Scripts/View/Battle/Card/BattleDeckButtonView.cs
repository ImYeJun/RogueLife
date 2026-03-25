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

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialDeckSettled>(OnInitialDeckSettled);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialDeckSettled>(OnInitialDeckSettled);
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

        public abstract void OnPointerClick(PointerEventData eventData);
    }
}
