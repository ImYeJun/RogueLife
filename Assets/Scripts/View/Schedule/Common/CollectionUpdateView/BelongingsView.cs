using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.ScheduleView.CollectionUpdateView
{
    public class BelongingsView : ItemView
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public void SetBelongings(Belongings belongings)
        {
            image.sprite = belongings.Image;
            nameText.text = belongings.Name;
            descriptionText.text = belongings.Description;

            PopUp();
        }
    }
}