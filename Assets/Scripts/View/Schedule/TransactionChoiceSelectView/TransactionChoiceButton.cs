using System;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.TransactionSelectView
{
    public class TransactionChoiceButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI subText;
        private Action onPressed;

        public void Initiate(TransactionChoiceData choiceData, Action onPressed)
        {
            Unactive();

            mainText.text = choiceData.Description;
            subText.text = choiceData.SubDescription;

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
            onPressed?.Invoke();
        }
    }
}