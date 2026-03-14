using UnityEngine;

namespace View.BattleView
{
    public interface IInspectable
    {
        public void OnInspect(IInspectorBuilder builder, RectTransform parent);
    }
}
