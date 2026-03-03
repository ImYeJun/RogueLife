using System;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace View.ScheduleView.Deck
{
    public class SortingSettingButton : MonoBehaviour
    {
        [SerializeField] private SortingType type;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Image background;
        [SerializeField] private Color32 activateBackgroundColor;
        [SerializeField] private Color32 deactivateBackgroundColor;

        public SortingType Type { get => type; }
        public Action<SortingType> onPressed;

        public void OnPressed()
        {
            onPressed.Invoke(type);
        }

        public void Activate(Order order)
        {
            string text = order switch
            {
                Order.Ascending => "↑",
                Order.Descending => "↓",
                _ => throw new InvalidOperationException($"[SortingSettingButton] {order} is not valid")
            };
            text += ConvertTypeToKorean();

            buttonText.text = text;
            background.color = activateBackgroundColor;
        }
        public void Deactivate()
        {
            var text = ConvertTypeToKorean();

            buttonText.text = text;
            background.color = deactivateBackgroundColor;
        }
        private string ConvertTypeToKorean()
        {
            return type switch
            {
                SortingType.ObtainDate => "획득 시기",
                SortingType.Name => "이름",
                SortingType.ActionCost => "코스트",
                _ => throw new InvalidOperationException($"[SortingSettingButton] {type} is not valid")
            };
        }

        public void Initialize()
        {
            Deactivate();
        }
    }
}
