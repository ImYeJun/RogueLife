using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using View.Core;
using ViewEvent.Core;
using ViewEvent.ScheduleView;

namespace View.ScheduleView.BattleNodes
{
    public class BattleNodeView : ViewBehaviour<IScheduleViewEvent>
    {
        [Header("References")]
        [SerializeField] private GameObject battleNodeView;
        [SerializeField] private Image mainEnemyUsualImage;
        [SerializeField] private EnemyLineView enemyLineView; 
        private EnemyData currentMainEnemy;

        [Header("Enter Presentation Settings")]
        [SerializeField] private float enterImageToLineDelay = 1f;
        [SerializeField] private float enterLineHoldDuration = 1f;

        [Header("Return Presentation Settings")]
        [SerializeField] private float returnImageHoldDuration = 2f;
        [SerializeField] private float returnLineHoldDuration = 2f;
        [SerializeField] private float returnImageFadeDuration = 1f;
        [SerializeField] private Ease returnFadeEase = Ease.InOutCubic;

        [Header("Test Settings")]
        [SerializeField] private EnemyData testEnemyData;
        [SerializeField] private bool testHasResolved;

        public override void OnInitialized()
        {
            battleNodeView.SetActive(false);
            mainEnemyUsualImage.gameObject.SetActive(false);
            enemyLineView.gameObject.SetActive(false);

            eventBus.Subscribe<NodeEntered>(OnBattleNodeEntered); 
            eventBus.Subscribe<ReturnedFromBattle>(OnReturnedFromBattle);
        }

        public override void OnDestroy()
        {
            eventBus?.Unsubscribe<NodeEntered>(OnBattleNodeEntered); 
            eventBus?.Unsubscribe<ReturnedFromBattle>(OnReturnedFromBattle);
        }   

        private void OnBattleNodeEntered(NodeEntered payload)
        {
            if (payload.EnteringNode is not BattleNode battleNode) { return; }

            currentMainEnemy = battleNode.MainEnemyData;

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_StageSet, BattleNodeEnterStageSetPresentation());
            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.NodeEnter_Specific, BattleNodeEnterPresentation());
        }

        private IEnumerator BattleNodeEnterStageSetPresentation()
        {
            battleNodeView.SetActive(true);
            
            mainEnemyUsualImage.sprite = currentMainEnemy.UsualSprite;
            mainEnemyUsualImage.gameObject.SetActive(true);
            
            var color = mainEnemyUsualImage.color;
            color.a = 1f;
            mainEnemyUsualImage.color = color;

            enemyLineView.SetLine(random, currentMainEnemy.EncounterLines);

            yield return null;
        }

        private IEnumerator BattleNodeEnterPresentation()
        {
            yield return new WaitForSeconds(enterImageToLineDelay);
            enemyLineView.gameObject.SetActive(true);
            yield return new WaitForSeconds(enterLineHoldDuration);
        }

        private void OnReturnedFromBattle(ReturnedFromBattle payload)
        {
            battleNodeView.SetActive(true);

            currentMainEnemy = payload.MainEnemyData;
            mainEnemyUsualImage.sprite = currentMainEnemy.UsualSprite;
            mainEnemyUsualImage.gameObject.SetActive(true);

            var lines = payload.HasResvoled ? currentMainEnemy.DefeatLines : currentMainEnemy.VictoryLines;
            enemyLineView.SetLine(random, lines);

            presentationManager.Enqueue(payload.SequenceId, PresentationPriority.ReturnedFromBattle_EnemyLine, ReturnedFromBattlePresentation());
        }

        private IEnumerator ReturnedFromBattlePresentation()
        {
            yield return new WaitForSeconds(returnImageHoldDuration);
            enemyLineView.gameObject.SetActive(true);
            yield return new WaitForSeconds(returnLineHoldDuration);
            enemyLineView.gameObject.SetActive(false);

            yield return mainEnemyUsualImage.DOFade(0f, returnImageFadeDuration).SetEase(returnFadeEase).WaitForCompletion();

            mainEnemyUsualImage.gameObject.SetActive(false);
            battleNodeView.SetActive(false);
        }

        [ContextMenu("Test Enter Presentation")]
        public void TestEnterPresentation()
        {
            StopAllCoroutines();
            StartCoroutine(TestEnterRoutine());
        }

        [ContextMenu("Test Return Presentation")]
        public void TestReturnPresentation()
        {
            StopAllCoroutines();
            StartCoroutine(TestReturnRoutine());
        }

        private IEnumerator TestEnterRoutine()
        {
            if (testEnemyData == null)
            {
                Debug.LogWarning("[BattleNodeView] Test Enemy Data가 비어있습니다! Inspector에 할당해주세요.");
                yield break;
            }

            yield return new WaitForSeconds(0.5f);

            currentMainEnemy = testEnemyData;
            mainEnemyUsualImage.sprite = currentMainEnemy.UsualSprite;
            var color = mainEnemyUsualImage.color; 
            color.a = 1f; 
            mainEnemyUsualImage.color = color;

            battleNodeView.SetActive(true);
            mainEnemyUsualImage.gameObject.SetActive(false);
            enemyLineView.gameObject.SetActive(false);

            yield return StartCoroutine(BattleNodeEnterPresentation());
        }

        private IEnumerator TestReturnRoutine()
        {
            if (currentMainEnemy == null && testEnemyData != null)
            {
                currentMainEnemy = testEnemyData;
            }

            if (currentMainEnemy == null)
            {
                Debug.LogWarning("[BattleNodeView] 현재 세팅된 Enemy Data가 없습니다. 먼저 Enter Test를 돌리거나 Test Enemy Data를 넣어주세요.");
                yield break;
            }

            yield return new WaitForSeconds(0.5f);

            battleNodeView.SetActive(true);
            mainEnemyUsualImage.gameObject.SetActive(true);
            enemyLineView.gameObject.SetActive(false);

            var color = mainEnemyUsualImage.color; 
            color.a = 1f; 
            mainEnemyUsualImage.color = color;

            var lines = testHasResolved ? currentMainEnemy.DefeatLines : currentMainEnemy.VictoryLines;
            enemyLineView.SetLine(random, lines);
            yield return StartCoroutine(ReturnedFromBattlePresentation());
        }
    }
}