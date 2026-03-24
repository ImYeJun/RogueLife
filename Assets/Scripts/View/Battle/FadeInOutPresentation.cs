using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Runtime.Remoting.Contexts;

namespace View.BattleView
{
    public class FadeInOutPresentation : ViewBehaviour<IBattleViewEvent>
    {
        [SerializeField] private Material tilingMaterial;
        [SerializeField] private Image foreground;
        private static readonly int ProgressID = Shader.PropertyToID("_Progress");
        
        [Header("Battle Started Presentation")]
        [SerializeField] private float battleStartFadeDuration;
        [SerializeField] private Ease battleStartFadeEasingType;

        [Header("Battle Exited Presentation")]
        [SerializeField] private float battleExitFadeDuration;
        [SerializeField] private Ease battleExitFadeEasingType;

        public override void OnInitialized()
        {
            foreground.gameObject.SetActive(true);
            eventBus.Subscribe<BattleStarted>(OnBattleStarted);
            eventBus.Subscribe<BattleExited>(OnBattleExited);
        }
        
        public override void OnDestroy()
        {
            KillActiveTweens();
            eventBus?.Unsubscribe<BattleStarted>(OnBattleStarted);
            eventBus?.Unsubscribe<BattleExited>(OnBattleExited);
        }

        private void OnBattleStarted(BattleStarted payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleStarted_FadeOut, BattleStartedPresentation());
        }
        
        private IEnumerator BattleStartedPresentation()
        {
            KillActiveTweens();
            foreground.material = tilingMaterial;
            tilingMaterial.SetFloat(ProgressID, 1);
            foreground.gameObject.SetActive(true);

            yield return tilingMaterial.DOFloat(0, ProgressID, battleStartFadeDuration).SetEase(battleStartFadeEasingType).WaitForCompletion();

            foreground.gameObject.SetActive(false);
            foreground.material = null;
        }

        private void OnBattleExited(BattleExited payload)
        {
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.BattleExited_FadeOut, BattleExitedPresentation());
        }
        
        private IEnumerator BattleExitedPresentation()
        {
            KillActiveTweens();
            foreground.material = tilingMaterial;
            
            tilingMaterial.SetFloat(ProgressID, 0); 
            foreground.gameObject.SetActive(true);

            yield return tilingMaterial.DOFloat(1, ProgressID, battleExitFadeDuration).SetEase(battleExitFadeEasingType).WaitForCompletion();
        }

        private void KillActiveTweens()
        {
            foreground.DOKill();
        }

#if UNITY_EDITOR
        [ContextMenu("Play On Battle Started Presentation")]
        public void TestOnBattleStarted()
        {
            presentationManager.Enqueue(0, PresentationPriority.BattleStarted_FadeOut, BattleStartedPresentation());
            foreground.gameObject.SetActive(true);
        }

        [ContextMenu("Play On Battle Exited Presentation")]
        public void TestOnBattleExited()
        {
            presentationManager.Enqueue(0, PresentationPriority.BattleExited_FadeOut, BattleExitedPresentation());
            foreground.gameObject.SetActive(true);
        }
#endif
    }
}