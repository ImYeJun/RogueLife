using System;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.IncidentSelectView
{
    public class IncidentChoiceButton : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI mainText;
        private Action onPressed;

        public void Initiate(DeterminedIncidentChoice choice, Action onPressed)
        {
            Unactive();

            mainText.text = choice.Description;
            this.onPressed = onPressed;
            
            gameObject.SetActive(true);
        }

        public void Unactive()
        {
            onPressed = null;
            gameObject.SetActive(false);
        }

        public void OnPressed()
        {
            if (onPressed == null) return;

            var actionToInvoke = onPressed;
            Unactive();

            actionToInvoke.Invoke();
        }
    }
}