using System;
using System.Collections;
using UnityEngine;
using ViewEvent.BattleView;

namespace View.BattleView
{
    public class BattlePlayerActionPresentation : MonoBehaviour 
    {
        // TODO: Refactor this code to reduce coupling
        [SerializeField] BattleEnemyViewController battleEnemyViewController;
        private IReadOnlyBattlePlayer player;
        private PresentationManager presentationManager;
        private Func<IEnumerator> playActionPresentation;

        public void Initiate(
            IReadOnlyBattlePlayer player, 
            PresentationManager presentationManager,
            Func<IEnumerator> playActionPresentation)
        {
            this.player = player;
            this.presentationManager = presentationManager;
            this.playActionPresentation = playActionPresentation;
        }

        public void OnCardEffectExecuted(CardEffectExecuted payload)
        {
            if (!payload.Caster.Caster.Equals(player))
            {
                throw new InvalidOperationException("[BattlePlayerActionPresentation/OnCardEffectExecuted] Card Caster is expected Player for now. But other entity executed a Card");
            }

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.CardEffectExecuted_CasterAction, CardExecutePresentation(payload));
        }
        
        private IEnumerator CardExecutePresentation(CardEffectExecuted payload)
        {
            Debug.Log($"{payload.ExecutedCard.CurrentName} 카드 효과 연출 실행");
            
            if (playActionPresentation != null)
            {
                yield return playActionPresentation.Invoke();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        } 
    }
}