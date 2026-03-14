using System;
using Battle.Enemies.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleEnemyActionIcon : MonoBehaviour, IPointerClickHandler, IInspectable
    {
        private EnemyAction action;

        public void Initialize(EnemyAction action)
        {
            this.action = action;
        }

        public void OnInspect(IInspectorBuilder builder, RectTransform parent)
        {
            var text = builder.AddNormalText(parent);
            text.Text = $"{action}";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (action is not null)
            {
                Debug.Log(action);
            }
        }
    }
}