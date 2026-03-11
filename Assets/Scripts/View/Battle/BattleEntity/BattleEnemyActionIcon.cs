using System;
using Battle.Enemies.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.BattleView
{
    public class BattleEnemyActionIcon : MonoBehaviour, IPointerClickHandler
    {
        private EnemyAction action;

        public void Initialize(EnemyAction action)
        {
            this.action = action;
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