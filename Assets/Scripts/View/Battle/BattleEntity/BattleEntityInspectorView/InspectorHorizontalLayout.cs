using UnityEngine;
using UnityEngine.UI;

namespace View.BattleView
{
    public class InspectorHorizontalLayout : MonoBehaviour {
        private HorizontalLayoutGroup layoutGroup;
        private RectTransform rectTransform;

        private void Awake() {
            layoutGroup = GetComponent<HorizontalLayoutGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        public HorizontalLayoutGroup LayoutGroup { get => layoutGroup; }
        public RectTransform RectTransform { get => rectTransform; }
    }
}