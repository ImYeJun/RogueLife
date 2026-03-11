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
            eventBus.Subscribe<PhaseIncreased>(OnPhaseIncreased);
            eventBus.Subscribe<PhaseDecreased>(OnPhaseDecreased);
        }
        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<InitialPhaseSettled>(OnInitialPhaseSettled);
            eventBus?.Unsubscribe<PhaseIncreased>(OnPhaseIncreased);
            eventBus?.Unsubscribe<PhaseDecreased>(OnPhaseDecreased);
        }

        public void OnPhaseIncreased(PhaseIncreased payload)
        {
            DrawPhaseText(payload.CurrentPhase);
        }
        public void OnPhaseDecreased(PhaseDecreased payload)
        {
            DrawPhaseText(payload.CurrentPhase);
        }

        public void OnInitialPhaseSettled(InitialPhaseSettled payload)
        {
            phase = payload.Phase;

            DrawPhaseText();
        }

        private void DrawPhaseText()
        {
            DrawPhaseText(phase.ReaminPhase);
        }
        private void DrawPhaseText(int currentPhase)
        {
            remainPhaseText.text = currentPhase.ToString();
        }
    }
}
