using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class AttributeFilterButton : MonoBehaviour
    {
        [SerializeField] private GameObject listView;
        [SerializeField] private List<AttributeFilterListButton> listButtons;

        private void Awake() {
            listView.SetActive(false);
        }
        public void OnPressed()
        {
            listView.SetActive(!listView.activeSelf);
        }

        public void SetState(HashSet<CardAttribute> attributes)
        {
            foreach (var listButton in listButtons)
            {
                listButton.SetState(attributes);
            }
        }

        public void SetOnListButtonPressed(Action<CardAttribute> toggleAttributeFilteringState)
        {
            foreach (var listButton in listButtons)
            {
                listButton.SetOnPressed(toggleAttributeFilteringState);
            }
        }

        public void Initialize()
        {
            listView.SetActive(false);

            SetState(new HashSet<CardAttribute>());
        }
    }
}