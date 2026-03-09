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

            viewCommander.StartBattle();
        }
    }
}