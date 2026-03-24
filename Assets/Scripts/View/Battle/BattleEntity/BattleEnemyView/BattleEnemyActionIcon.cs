using System;
using System.Collections;
using Battle.Enemies.Actions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

namespace View.BattleView
{
    public class BattleEnemyActionIcon : MonoBehaviour
    {
        [Serializable]
        public struct ActionTypeSpriteMap
        {
            public BattleEnemyActionType type;
            public Sprite sprite;
        }

        private CanvasGroup canvasGroup;
        private LayoutElement layoutElement;

        private EnemyAction action;
        public EnemyAction Action => action;

        [Header("Sprite")]
        [SerializeField] private Image typeImage;
        [SerializeField] private Image indexImage;
        [SerializeField] private List<ActionTypeSpriteMap> actionTypeSpriteMap;
        [SerializeField] private List<Sprite> actionIndexSprite;
        

        [Header("Applied Presentation")]
        [SerializeField] private float appliedPresentationDuration;
        [SerializeField] private Ease appliedPresentationEase;

        [Header("Executed Presentation")]
        [SerializeField] private float executedPresentationDuration;
        [SerializeField] private Ease executedPresentationEase;
        [SerializeField] private Vector3 punchAmount;
        [SerializeField] private int punchVibrato;
        [SerializeField] private float punchElasticity;

        [Header("Removed Presentation")]
        [SerializeField] private float removedPresentationDuration;
        [SerializeField] private Ease removedPresentationEase;

        public bool HasExecuted { get; set;}

        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
            layoutElement = GetComponent<LayoutElement>();

            canvasGroup.alpha = 0;
            layoutElement.ignoreLayout = true;
        }

        public void Initialize(EnemyAction action)
        {
            this.action = action;

            var typeSprite = actionTypeSpriteMap.FirstOrDefault(map => map.type == action.ActionType).sprite;
            typeImage.sprite = typeSprite;

            //TODO Refactor the index to be a attribute of EnemyAction
            string id = action.Id;
            int index =  int.Parse(id[id.Length - 1].ToString());
            var indexSprite = actionIndexSprite[index];
            indexImage.sprite = indexSprite;    
        }

        public IEnumerator PlayAppliedPresentation()
        {
            layoutElement.ignoreLayout = false;
            canvasGroup.alpha = 0;
            yield return canvasGroup.DOFade(1, appliedPresentationDuration).SetEase(appliedPresentationEase).WaitForCompletion();
        }

        public IEnumerator PlayExecutedPresentation()
        {
            yield return transform.DOPunchScale(punchAmount, executedPresentationDuration, punchVibrato, punchElasticity).SetEase(executedPresentationEase).WaitForCompletion();
        }

        public IEnumerator PlayRemovedPresentation()
        {
            canvasGroup.alpha = 1;
            yield return canvasGroup.DOFade(0, removedPresentationDuration).SetEase(removedPresentationEase).WaitForCompletion();
        }

#if UNITY_EDITOR
        [ContextMenu("Play Applied Presentation")]
        public void TestAppliedPresentation()
        {
            canvasGroup.alpha = 0;
            StartCoroutine(DelayTestPlay(PlayAppliedPresentation()));
        }

        [ContextMenu("Play Executed Presentation")]
        public void TestExecutedPresentation()
        {
            StartCoroutine(DelayTestPlay(PlayExecutedPresentation()));
        }

        [ContextMenu("Play Removed Presentation")]
        public void TestRemovedPresentation()
        {
            canvasGroup.alpha = 1;
            StartCoroutine(DelayTestPlay(PlayRemovedPresentation()));
        }

        private IEnumerator DelayTestPlay(IEnumerator presentation)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(presentation);
        }
#endif
    }
}