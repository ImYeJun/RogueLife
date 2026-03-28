using System;
using TMPro;
using UnityEngine;

namespace View.ScheduleView
{
    public abstract class DoubleTextSelectButton : SelectButton
    {
        [Header("Text Format")]
        [SerializeField] protected TextMeshProUGUI mainDescription;
        [SerializeField] protected TextMeshProUGUI subDescription;

        protected void Initialize(Action onPressed, string mainText, string subText)
        {
            InitAction(onPressed);
            
            if (mainDescription != null) mainDescription.text = mainText;
            if (subDescription != null) subDescription.text = subText;
        }
    }
}