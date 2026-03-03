using System;
using System.Collections.Generic;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class SortingSettingView : MonoBehaviour {
        [SerializeField] private List<SortingSettingButton> buttons;

        public void SetState(SortingState state)
        {
            foreach (var button in buttons)
            {
                if (button.Type == state.Type)
                {
                    button.Activate(state.Order);
                }
                else
                {
                    button.Deactivate();
                }
            }
        }
        public void SetOnButtonPressed(Action<SortingType> changeSortingType)
        {
            foreach (var button in buttons)
            {
                button.onPressed = changeSortingType;
            }
        }

        public void Initialize()
        {
            foreach (var button in buttons)
            {
                button.Initialize();
            }
        }
    }
}