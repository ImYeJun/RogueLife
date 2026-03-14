using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ViewEvent.BattleView;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleStatusEffectIcon : MonoBehaviour, IInspectable, IPointerClickHandler
    {
        [SerializeField] private Image effectImage;
        [SerializeField] private TextMeshProUGUI stackText;
        [SerializeField] private TextMeshProUGUI remainTurnText;
        private IReadOnlyBattleStatusEffect currentEffect;
        public IReadOnlyBattleStatusEffect CurrentEffect => currentEffect;

        public void Initialize(IReadOnlyBattleStatusEffect effect)
        {
            if (effect == null)
            {
                throw new ArgumentNullException("[BattleStatusEffectIcon/Initialize] The given status effect is null.");
            }

            currentEffect = effect;
            effectImage.sprite = effect.Data.Icon;

            UpdateState(effect.RemainTurn, effect.StackCount);
        }

        public void UpdateState(int remainTurn, int currentStack)
        {
            stackText.text = currentStack > 1 ? $"x{currentStack}" : "";
            remainTurnText.text = currentEffect.IsDurationEternal ? "" : remainTurn.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentEffect is not null)
            {
                Debug.Log($"{currentEffect.Data.Id}");
            }
        }

        public void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var normalText = builder.AddNormalText(parent);
            normalText.Text = $"({currentEffect.Data.Name}) 스택 : {currentEffect.StackCount}, 남은 턴  : {currentEffect.RemainTurn}";
        }
    }
}