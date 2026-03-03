using System;
using System.Collections.Generic;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class TypeFilterButton : MonoBehaviour
    {
        [SerializeField] private GameObject listView;
        [SerializeField] private List<TypeFilterListButton> listButtons;
        
        private void Awake() {
            listView.SetActive(false);
        }
        public void OnPressed()
        {
            listView.SetActive(!listView.activeSelf);
        }

        public void SetState(HashSet<CardType> types)
        {
            foreach (var listButton in listButtons)
            {
                listButton.SetState(types);
            }
        }

        public void SetOnListButtonPressed(Action<CardType> toggleAttributeFilteringState)
        {
            foreach (var listButton in listButtons)
            {
                listButton.SetOnPressed(toggleAttributeFilteringState);
            }
        }

        public void Initialize()
        {
            listView.SetActive(false);
            SetState(new HashSet<CardType>());
        }
    }
}