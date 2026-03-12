using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System.Collections;

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

        private void OnPlayerTurnStarted(PlayerTurnStarted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnStarted_TurnEndButtonShow, ShowPresentation(),
            () => { gameObject?.SetActive(true); } );
        }
        private IEnumerator ShowPresentation()
        {
            yield return null;
        }

        private void OnPlayerTurnEnded(PlayerTurnEnded payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerTurnEnded_TurnViewDisappearingUp, DisappearPresentation(),
            () => { gameObject?.SetActive(false); } );
        }
        private IEnumerator DisappearPresentation()
        {
            yield return null;
        }

        public void OnPressed()
        {
            commander.EndPlayerTurn();
        }
    }
}
