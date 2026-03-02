using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView
{
    public class CardInspectorView : ViewBehaviour<IScheduleViewEvent>
    {
        private Card currentCard;

        [SerializeField] private SharedCardView cardView;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardAttribute;
        [SerializeField] private TextMeshProUGUI cardType;
        [SerializeField] private TextMeshProUGUI cardEffectDescription;
        [SerializeField] private GameObject effectTypeButtonsView;

        public override void OnInitialized()
        {
            SetViewActive(false);
        }
        public void SetViewActive(bool value)
        {
            cardView.gameObject.SetActive(value);
            cardName.gameObject.SetActive(value);
            cardAttribute.gameObject.SetActive(value);
            cardType.gameObject.SetActive(value);
            cardEffectDescription.gameObject.SetActive(value);
            effectTypeButtonsView.SetActive(value);
        }
        public override void OnDestroy()
        {
        }

        public void VisualizeSelectedSlot(Card card)
        {
            currentCard = card;
            
            cardView.SetCard(card);

            cardName.text = card.CurrentName;
            cardAttribute.text = $"속성 : ㅁㅁ ({CardAttributeExtensions.ToKorean(card.CurrentAttribute)})";
            cardType.text = $"유형 : {CardTypeExtensions.ToKorean(card.CurrentType)}";
            ShowNormalDescription();

            SetViewActive(true);
        }
        public void ShowNormalDescription()
        {
            cardEffectDescription.text = $"효과 : \n {currentCard.NormalEffectDescription}";
        }
        public void ShowReflectionDescription()
        {
            cardEffectDescription.text = $"효과 : \n {currentCard.ReflectionEffectDescription}";
        }
    }
}
