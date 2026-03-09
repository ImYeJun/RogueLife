using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.BattleView;
using TMPro;
using System;

namespace View.BattleView
{
    public class RemainPhaseView : ViewBehaviour<IBattleViewEvent>
    {
        private IReadOnlyBattlePhase phase;
        [SerializeField] private TextMeshProUGUI remainPhaseText;

        public override void OnInitialized()
        {
            eventBus.Subscribe<InitialPhaseSettled>(OnInitialPhaseSettled);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialPhaseSettled>(OnInitialPhaseSettled);
        }

        public void OnInitialPhaseSettled(InitialPhaseSettled payload)
        {
            phase = payload.Phase;

            DrawPhaseText();
        }

        private void DrawPhaseText()
        {
            remainPhaseText.text = phase.ReaminPhase.ToString();
        }
    }
}
