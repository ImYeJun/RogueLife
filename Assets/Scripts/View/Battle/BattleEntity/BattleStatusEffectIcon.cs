using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // ⭐️ 수정된 부분: 텍스트 UI를 위해 추가
using ViewEvent.BattleView;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleStatusEffectIcon : MonoBehaviour, IPointerEnterHandler
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (currentEffect is not null)
            {
                Debug.Log($"{currentEffect.Data.Id}");
            }
        }
    }
}