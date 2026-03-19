using System.Collections;
using DG.Tweening;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class HealthUpdatePresentation : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private BattleHealthView battleHealthView;
        [SerializeField] private MentalityView mentalityView;
        [SerializeField] private PlayerStatusView playerStatusView;
        [SerializeField] private PlayerImageView playerImageView;

        [Header("Global Tween Settings")]
        [SerializeField] private float totalFillDuration = 0.4f;
        [SerializeField] private float textOffsetDuration = 0.2f;

        public override void OnInitialized()
        {
            if (battleHealthView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] battleHealthView is not assigned.");
            if (mentalityView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] mentalityView is not assigned.");
            if (playerStatusView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] playerStatusView is not assigned.");
            if (playerImageView == null) Debug.LogWarning("[HealthUpdatePresentation/OnInitialized] playerImageView is not assigned.");

            eventBus.Subscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus.Subscribe<PlayerHurt>(OnPlayerHurt);
            eventBus.Subscribe<PlayerHealed>(OnPlayerHealed);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<ScheduleStateSynced>(OnScheduleStateSynced);
            eventBus?.Unsubscribe<PlayerHurt>(OnPlayerHurt);
            eventBus?.Unsubscribe<PlayerHealed>(OnPlayerHealed);
        }

        private void OnScheduleStateSynced(ScheduleStateSynced payload)
        {
            if (battleHealthView != null) battleHealthView.DrawViewInstant(payload.Health);
            if (mentalityView != null) mentalityView.DrawViewInstant(payload.Health);
            if (playerStatusView != null) playerStatusView.DrawViewInstant(payload.Health);
        }

        private void OnPlayerHurt(PlayerHurt payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHurt, HurtPresentationRoutine(payload));
        }

        private void OnPlayerHealed(PlayerHealed payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.PlayerHealed, HealPresentationRoutine(payload));
        }

        private IEnumerator HurtPresentationRoutine(PlayerHurt payload)
        {
            Sequence mainSeq = DOTween.Sequence();

            float totalDamage = payload.BattleHealthDamage + payload.MentalityDamage;
            float battleDur = totalDamage > 0 ? totalFillDuration * ((float)payload.BattleHealthDamage / totalDamage) : totalFillDuration;
            float mentDur = totalDamage > 0 ? totalFillDuration * ((float)payload.MentalityDamage / totalDamage) : totalFillDuration;

            Sequence battleHealthSeq = DOTween.Sequence();
            if (payload.BattleHealthDamage > 0)
            {
                battleHealthSeq.Join(battleHealthView.GetUpdateHealthTween(payload.CurrentBattleHealth, payload.MaxBattleHealth, battleDur, textOffsetDuration));
            }

            Sequence mentalitySeqence = DOTween.Sequence();
            if (payload.MentalityDamage > 0)
            {
                mentalitySeqence.Join(mentalityView.GetUpdateMentalityTween(payload.CurrentMentality, payload.MaxMentality, mentDur, textOffsetDuration));
                mentalitySeqence.Join(playerStatusView.GetUpdateSliderTween(payload.CurrentMentality, payload.MaxMentality, mentDur));
            }

            mainSeq.Join(battleHealthSeq);
            if (payload.IsOverflowed)
            {
                mainSeq.Insert(battleDur, mentalitySeqence);
                battleHealthSeq.Join(playerImageView.GetHurtEffectTween());
            }
            else
            {
                mainSeq.Join(mentalitySeqence);
                battleHealthSeq.Join(playerImageView.GetHurtEffectTween());
            }

            yield return mainSeq.WaitForCompletion();
        }

        private IEnumerator HealPresentationRoutine(PlayerHealed payload)
        {
            Sequence mainSeq = DOTween.Sequence();

            float totalHeal = payload.BattleHealtHeal + payload.MentalityHeal;
            float battleDur = totalHeal > 0 ? totalFillDuration * ((float)payload.BattleHealtHeal / totalHeal) : totalFillDuration;
            float mentDur = totalHeal > 0 ? totalFillDuration * ((float)payload.MentalityHeal / totalHeal) : totalFillDuration;

            Sequence mentalitySeqence = DOTween.Sequence();
            if (payload.MentalityHeal > 0)
            {
                mentalitySeqence.Join(mentalityView.GetUpdateMentalityTween(payload.CurrentMentality, payload.MaxMentality, mentDur, textOffsetDuration));
                mentalitySeqence.Join(playerStatusView.GetUpdateSliderTween(payload.CurrentMentality, payload.MaxMentality, mentDur));
            }

            Sequence battleHealthSeq = DOTween.Sequence();
            if (payload.BattleHealtHeal > 0)
            {
                battleHealthSeq.Join(battleHealthView.GetUpdateHealthTween(payload.CurrentBattleHealth, payload.MaxBattleHealth, battleDur, textOffsetDuration));
            }

            mainSeq.Join(mentalitySeqence);
            if (payload.IsOverflowed)
            {
                mainSeq.Insert(mentDur, battleHealthSeq);
            }
            else
            {
                mainSeq.Join(battleHealthSeq);
            }

            yield return mainSeq.WaitForCompletion();
        }
    }
}