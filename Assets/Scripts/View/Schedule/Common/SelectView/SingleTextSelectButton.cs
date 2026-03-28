using System;
using TMPro;
using UnityEngine;

namespace View.ScheduleView
{
    public abstract class SingleTextSelectButton : SelectButton
    {
        [Header("Text Format")]
        [SerializeField] protected TextMeshProUGUI mainDescription;

        protected void Initialize(Action onPressed, string mainText)
        {
            InitAction(onPressed);
            
            if (mainDescription != null) 
                mainDescription.text = mainText;
        }
    }
}