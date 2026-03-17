using UnityEngine;

namespace View.BattleView
{
    public class InspectorLinkedGroup : MonoBehaviour{
        [SerializeField] private RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;
    }
}
