using System.Collections.Generic;
using Battle.Enemies.Actions;
using UnityEngine;

namespace View.BattleView
{
    public class InspectorEnemyActionInfoPanel : MonoBehaviour 
    {
        [SerializeField] private RectTransform itemContainer;
        [SerializeField] private BattleEnemyActionIcon actionIcon;

        public void Initialize(EnemyAction action, string description, List<string> relatedBattleStatusEffects, IInspectorBuilder builder)
        {
            actionIcon.Initialize(action);

            var availableBehaviourText = builder.AddBodyText(itemContainer);
            availableBehaviourText.Text = description;

            foreach (var effectString in relatedBattleStatusEffects)
            {
                var statusEffectText = builder.AddCaptionText(itemContainer);
                statusEffectText.Text = effectString;
            }
        }

        public RectTransform ItemContainer => itemContainer;
    }
}