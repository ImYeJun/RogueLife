using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.Deck
{
    public class AttributeFilterListButton : MonoBehaviour
    {
        [SerializeField] private CardAttribute attribute;
        [SerializeField] private TextMeshProUGUI buttonText;
        private Action<CardAttribute> onPressed;

        public void OnPressed() => onPressed.Invoke(attribute);

        public void SetOnPressed(Action<CardAttribute> toggleAttributeFilteringState)
        {
            onPressed = toggleAttributeFilteringState; 
        }

        public void SetState(HashSet<CardAttribute> attributes)
        {
            string symbol = attributes.Contains(attribute) ? "●" : "○";
            string attributeName = CardAttributeExtensions.ToKorean(attribute);
            buttonText.text = $"{symbol} {attributeName}";
        }
    }
}