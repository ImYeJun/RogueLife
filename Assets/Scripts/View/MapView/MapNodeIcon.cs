using UnityEngine;
using UnityEngine.UI;

namespace View.ScheduleView.Map
{
    public class MapNodeIcon : MonoBehaviour {
        [Header("Node Icons")]
        [SerializeField] private Sprite entryIconSprite;
        [SerializeField] private Sprite battleIconSprite;
        [SerializeField] private Sprite bossIconSprite;
        [SerializeField] private Sprite incidentIconSprite;
        [SerializeField] private Sprite transactionIconSprite;

        [Header("Image Components")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        
        private Sprite selectedSprite;

        public void Initiate(Node node)
        {
            selectedSprite = node switch{
                ScheduleEntryNode => entryIconSprite,
                BattleNode battleNode => battleNode.IsBossNode ? bossIconSprite : battleIconSprite,
                IncidentNode => incidentIconSprite,
                TransactionNode => transactionIconSprite,
                _ => null
            };

            icon.sprite = selectedSprite;
        }
    }
}
