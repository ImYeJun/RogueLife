using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using View.Core;
using View.ScheduleView.BattleNodes;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.Presentation
{
    public class BattleNodePresentation : ViewBehaviour<IScheduleViewEvent>
    {
        [SerializeField] private GameObject battleNodeView;
        [SerializeField] private Image mainEnemyUsualImage;
        [SerializeField] private EnemyEncounterLineView enemyEncounterLineView; 

        public override void OnInitialized()
        {
            battleNodeView.SetActive(false);
            mainEnemyUsualImage.gameObject.SetActive(false);
            enemyEncounterLineView.gameObject.SetActive(false);

            eventBus.Subscribe<NodeEntered>(OnBattleNodeEntered); 
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<NodeEntered>(OnBattleNodeEntered); 
        }   

        public void OnBattleNodeEntered(NodeEntered payload)
        {
            if (payload.EnteringNode is not BattleNode battleNode) { return; }

            battleNodeView.SetActive(true);

            var mainEnemy = battleNode.MainEnemyData;
            
            mainEnemyUsualImage.sprite = mainEnemy.UsualSprite;
            enemyEncounterLineView.Initiate(random, mainEnemy.EncounterLines);

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_Specific, BattleNodeEnterPresentation());
        }

        public IEnumerator BattleNodeEnterPresentation()
        {
            mainEnemyUsualImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(1);
            enemyEncounterLineView.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(1);
        }
    }
}
