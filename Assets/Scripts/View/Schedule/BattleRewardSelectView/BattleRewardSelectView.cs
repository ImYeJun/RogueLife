using System.Collections.Generic;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class BattleRewardSelectView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
    {
        [SerializeField] private GameObject uiRoot;
        [SerializeField] private GameObject rewardButtonPrefab;
        [SerializeField] private Transform buttonsContainer;
        [SerializeField] private GameObject rewardCancleButton;
        private List<BattleRewardButton> activeRewardIcons = new List<BattleRewardButton>();

        public override void OnInitialized()
        {
            uiRoot.SetActive(false);
            var buttonBehaviour = rewardCancleButton.GetComponent<BattleRewardButton>();
            buttonBehaviour.Initiate(null, OnButtonSelected, commander);

            eventBus.Subscribe<BattleRewardSelectRequested>(OnBattleRewardSelectRequested);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<BattleRewardSelectRequested>(OnBattleRewardSelectRequested);
        }

        private void OnBattleRewardSelectRequested(BattleRewardSelectRequested payload)
        {
            foreach (var icon in activeRewardIcons)
            {
                Destroy(icon.gameObject);
            }
            activeRewardIcons.Clear();

            var candidates = payload.RewardCollector.RewardCandidates;
            foreach (var candidate in candidates)
            {
                var rewardGameObject = Instantiate(rewardButtonPrefab, buttonsContainer);

                var button = rewardGameObject.GetComponent<BattleRewardButton>();
                button.Initiate(candidate, OnButtonSelected, commander);

                button.transform.SetAsLastSibling();
            }
            rewardCancleButton.transform.SetAsLastSibling();

            uiRoot.SetActive(true);
        }

        private void OnButtonSelected()
        {
            uiRoot.SetActive(false);
        }
    }
}
