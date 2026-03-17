using UnityEngine;

namespace View.BattleView{
    public interface IInspectorBuilder
    {
        public InspectorBodyText AddBodyText(RectTransform parent);
        public InspectorCaptionText AddCaptionText(RectTransform parent);
        public InspectorHeader AddHeader(RectTransform parent);
        public InspectorLinkedGroup AddLinkedGroup(RectTransform parent);
        public InspectorNameText AddNameText(RectTransform parent);
        public InspectorSubPanel AddSubPanel(RectTransform parent);
    }
}
