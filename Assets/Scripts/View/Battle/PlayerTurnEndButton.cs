using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class PlayerTurnEndButton : InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>
    {
        public override void OnInitialized()
        {
            gameObject.SetActive(false);
            eventBus.Subscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus.Subscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<PlayerTurnStarted>(OnPlayerTurnStarted);
            eventBus?.Unsubscribe<PlayerTurnEnded>(OnPlayerTurnEnded);
        }

        public void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            gameObject.SetActive(true);
        }

        public void OnPlayerTurnEnded(PlayerTurnEnded payload)
        {
            gameObject.SetActive(false);
        }

        public void OnPressed()
        {
            commander.EndPlayerTurn();
        }
    }
}
