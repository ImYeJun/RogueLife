using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class CostFilteringButton : MonoBehaviour
    {
        [SerializeField] private GameObject listView;
        [SerializeField] private List<CostFilterListButton> listButtons;

        private void Awake() {
            listView.SetActive(false);
        }
        public void OnPressed()
        {
            listView.SetActive(!listView.activeSelf);
        }

        public void SetState(HashSet<int> filteringCost)
        {
            foreach (var button in listButtons)
            {
                button.SetState(filteringCost);
            }
        }

        public void SetOnListButtonPressed(Action<int> toggleCostFilteringState)
        {
            foreach (var button in listButtons)
            {
                button.SetOnPressed(toggleCostFilteringState);
            }
        }

        public void Initialize()
        {
            listView.SetActive(false);
            SetState(new HashSet<int>());
        }
    }
}