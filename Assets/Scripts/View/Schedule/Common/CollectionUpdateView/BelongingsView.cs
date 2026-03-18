using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.ScheduleView.CollectionUpdateView
{
    public class BelongingsView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        public Action OnClicked;

        public void SetBelongings(Belongings belongings)
        {
            image.sprite = belongings.Image;
            nameText.text = belongings.Name;
            descriptionText.text = belongings.Description;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}