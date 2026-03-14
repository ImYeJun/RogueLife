using UnityEngine;

namespace View.BattleView{
    public interface IInspectorBuilder
    {
        public InspectorMainText AddMainText(RectTransform parent);
        public InspectorNameText AddNameText(RectTransform parent);
        public InspectorNormalText AddNormalText(RectTransform parent);
        public InspectorSubPanel AddSubPanel(RectTransform parent);
    }
}
