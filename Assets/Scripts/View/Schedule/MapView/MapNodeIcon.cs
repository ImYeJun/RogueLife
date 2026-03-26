using System;
using UnityEngine;
using UnityEngine.UI;

namespace View.ScheduleView.Map
{
    public class MapNodeIcon : MonoBehaviour 
    {
        [Header("Node Icons")]
        [SerializeField] private Sprite entryIconSprite;
        [SerializeField] private Sprite normalEnemyIconSprite;
        [SerializeField] private Sprite eliteIconSprite;
        [SerializeField] private Sprite bossIconSprite;
        [SerializeField] private Sprite incidentIconSprite;
        [SerializeField] private Sprite transactionIconSprite;

        [Header("Image Components")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        
        private Node currentNode;
        private Sprite selectedSprite;

        public Node CurrentNode { get => currentNode; }

        public void Initiate(Node node)
        {
            currentNode = node;

            selectedSprite = currentNode switch {
                ScheduleEntryNode => entryIconSprite,
                BattleNode battleNode => battleNode.MainEnemyData.Tier switch
                {
                    EnemyTier.NORMAL => normalEnemyIconSprite,
                    EnemyTier.ELITE => eliteIconSprite,
                    EnemyTier.BOSS => bossIconSprite,
                    _ => throw new InvalidOperationException($"[MapNodeIcon/Initiate] {battleNode.MainEnemyData.Tier} is not valid")
                },
                IncidentNode => incidentIconSprite,
                TransactionNode => transactionIconSprite,
                _ => null
            };

            icon.sprite = selectedSprite;
            OnUnfocused();
        }

        public void OnFocused()
        {
            background.color = Color.yellow;
        }
        
        public void OnUnfocused()
        {
            background.color = Color.white;
        }
    }
}