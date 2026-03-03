using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class TypeFilterListButton : MonoBehaviour
    {

        [SerializeField] private CardType type;
        [SerializeField] private TextMeshProUGUI buttonText;
        private Action<CardType> onPressed;

        public void OnPressed() => onPressed.Invoke(type);

        public void SetOnPressed(Action<CardType> toggleTypeFilteringState)
        {
            onPressed = toggleTypeFilteringState; 
        }

        public void SetState(HashSet<CardType> attributes)
        {
            string symbol = attributes.Contains(type) ? "●" : "○";
            string attributeName = CardTypeExtensions.ToKorean(type);
            buttonText.text = $"{symbol} {attributeName}";
        }
    }
}