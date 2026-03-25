using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.BattleView;

namespace Controller.Battle
{
    public class BattleRootController : InGameRootController
    {
        private BattleViewEventBus viewEventBus;
        private IBattleViewCommander viewCommander;

        [SerializeField] private AudioData normalBgm;
        [SerializeField] private List<ViewBehaviour<IBattleViewEvent>> views;
        [SerializeField] private List<InteractableViewBehaviour<IBattleViewEvent, IBattleViewCommander>> interacatbleViews;
        
        protected override void OnInitialize()
        { 
            viewCommander = currentRun.BattleViewCommander;
            viewEventBus = currentRun.BattleViewEventBus;

            foreach (var view in views)
            {
                view.Initialize(random, viewEventBus, PresentationManager.Instance);
            }
            foreach (var interactabelView in interacatbleViews)
            {
                interactabelView.Initialize(random, viewEventBus, PresentationManager.Instance ,viewCommander);
            }

            viewEventBus.Subscribe<BattleStarted>(OnBattleStarted);
            viewEventBus.Subscribe<BattleExited>(OnBattleExited);

            viewCommander.StartBattle();
        }
        private void OnDestroy()
        {
            viewEventBus?.Unsubscribe<BattleStarted>(OnBattleStarted);
            viewEventBus?.Unsubscribe<BattleExited>(OnBattleExited);
        }

        private void OnBattleStarted(BattleStarted payload)
        {
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.BattleStarted_PlayBgm, PlayBgm(payload.MainEnemyData));
        }
        private IEnumerator PlayBgm(EnemyData mainEnemyData)
        {
            yield return null;
            
            var bgm = normalBgm;
            if (mainEnemyData is BossEnemyData bossEnemyData)
            {
                bgm = bossEnemyData.BattleBgm;
            }
            SoundManager.Instance?.PlayeBgm(bgm);
        }

        private void OnBattleExited(BattleExited payload)
        {
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.BattleExited_StopBgm, StopBgm());
            PresentationManager.Instance.Enqueue(payload.SequenceId, PresentationPriority.BattleExited_SceneTransition, SceneTransitionPresentation());
        }

        private IEnumerator StopBgm()
        {
            yield return null;
            SoundManager.Instance?.StopBgm();
        }

        private IEnumerator SceneTransitionPresentation()
        {
            yield return null;
            GameSceneManager.Instance.LoadScene(SceneName.SCHEDULE);
        }
    }
}