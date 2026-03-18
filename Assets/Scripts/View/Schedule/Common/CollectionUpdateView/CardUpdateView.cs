using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.CollectionUpdateView
{
    public class CardUpdateView : MonoBehaviour
    {
        [SerializeField] private GameObject updateViewObject;
        [SerializeField] private CardView cardView;
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI reflectionButtonText;
        private Action OnUpdateConfirmed;

        private void Awake() {
            updateViewObject.SetActive(false);
            cardView.OnClicked = OnCardClicked;
        }

        public void OnObatined(CardObtained payload, Action onUpdateConfirmed)
        {
            updateViewObject.SetActive(true);
            OnUpdateConfirmed = onUpdateConfirmed;
            cardView.Draw(payload.Card);

            header.text = "카드 획득";
            SetReflectionButtonText(cardView.IsReflectionText);
        }

        public void OnRemoved(CardRemoved payload, Action onUpdateConfirmed)
        {
            updateViewObject.SetActive(true);
            OnUpdateConfirmed = onUpdateConfirmed;
            cardView.Draw(payload.Card);

            header.text = "카드 상실";
            SetReflectionButtonText(cardView.IsReflectionText);
        }

        public void OnCardClicked()
        {
            gameObject.SetActive(false);
            OnUpdateConfirmed?.Invoke();
        }

        public void OnReflectionTextButtonClicked()
        {
            cardView.DrawDescription(!cardView.IsReflectionText);
            SetReflectionButtonText(cardView.IsReflectionText);
        }

        private void SetReflectionButtonText(bool isReflection)
        {
            reflectionButtonText.text = isReflection ? "복기 효과 보기" : "기본 효과 보기";
        }

        public void SetActive(bool value)
        {
            updateViewObject.SetActive(value);
        }
    }
}