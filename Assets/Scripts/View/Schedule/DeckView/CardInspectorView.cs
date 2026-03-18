using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Deck
{
    public class CardInspectorView : InteractableViewBehaviour<IScheduleViewEvent, IScheduleViewCommander>
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

        //* Referenced by DeckInventoryView in UnityEvent 
        public void VisualizeSelectedSlot(Card card)
        {
            currentCard = card;
            
            cardView.SetCard(card);

            cardName.text = card.CurrentName;
            cardAttribute.text = $"속성 : <sprite index={CardAttributeExtensions.GetTextIconIndex(card.CurrentAttribute)}> ({CardAttributeExtensions.ToKorean(card.CurrentAttribute)})";
            cardType.text = $", 유형 : {CardTypeExtensions.ToKorean(card.CurrentType)}";
            ShowNormalDescription();

            SetViewActive(true);
        }

        public void ShowNormalDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(currentCard.NormalEffectDescription);
            AppendStatusEffectDescriptions(sb, currentCard.RelatedStatusEffectIds);
            cardEffectDescription.text = sb.ToString();

            cardView.DrawDescription(false);
        }

        public void ShowReflectionDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(currentCard.ReflectionEffectDescription);
            AppendStatusEffectDescriptions(sb, currentCard.ReflectionRelatedStatusEffectIds);
            cardEffectDescription.text = sb.ToString();

            cardView.DrawDescription(true);
        }

        private void AppendStatusEffectDescriptions(StringBuilder sb, IReadOnlyList<string> statusEffectIds)
        {
            if (statusEffectIds == null || statusEffectIds.Count == 0) return;

            sb.Append("\n<size=70%>");
            foreach (var relatedStatusEffect in statusEffectIds)
            {
                var data = commander.GetStatusEffectData(relatedStatusEffect);
                if (data != null)
                {
                    sb.Append($"({data.Name})").Append("\n").Append(data.Description).Append("\n"); 
                }
            }
            sb.Append("</size>");
        }
    }
}
