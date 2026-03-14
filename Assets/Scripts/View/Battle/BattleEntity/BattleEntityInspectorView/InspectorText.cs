using TMPro;
using UnityEngine;

namespace View.BattleView
{
    public abstract class InspectorText : MonoBehaviour {
        private TextMeshProUGUI textMeshPro;

        public string Text { get => textMeshPro.text; set => textMeshPro.text = value; }

        private void Awake() {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }
}