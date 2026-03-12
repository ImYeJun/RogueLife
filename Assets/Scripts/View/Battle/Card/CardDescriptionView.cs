using System;
using TMPro;
using UnityEngine;

namespace View.BattleView
{
    public class CardDescriptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private float floatingDistance;

        public void Focus(BattleCardView focusedCardView)
        {
            var floatingPosition = focusedCardView.gameObject.transform.localPosition + Vector3.up * floatingDistance;
            transform.localPosition = floatingPosition;

            description.text = focusedCardView.Card.CurrentDescription;
            gameObject.SetActive(true);
        }

        public void Unfocus()
        {
            gameObject.SetActive(false);
        }
    }
}