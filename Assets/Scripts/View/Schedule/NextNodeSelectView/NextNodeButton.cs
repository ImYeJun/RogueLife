using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.NextNodeSelectView
{
    public class NextNodeButton : MonoBehaviour
    {
        public enum NodeDirection { Left, Middle, Right }
        
        private const string LEFT_NODE_MAIN_DESCRIPTION = "왼쪽으로 가기";
        private const string MIDDLE_NODE_MAIN_DESCRIPTION = "직진하기";
        private const string RIGHT_NODE_MAIN_DESCRIPTION = "오른쪽으로 가기";

        [Header("Behaviour")]
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI subText;

        [Header("Presentation")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform itemsContainer;
        [SerializeField] private float initiateDuration;
        [SerializeField] private Ease initiateEasingType;

        private Node node;
        private Action<Node> OnNextNodeSelected;
        private Tween initiateTween;

        public void Initiate(int sequenceId, PresentationManager presentationManager, NodeDirection direction, Node node, Action<Node> onNextNodeSelected)
        {
            itemsContainer.gameObject.SetActive(false);

            this.node = node;
            this.OnNextNodeSelected = onNextNodeSelected;

            mainText.text = GetMainText(direction);
            subText.text = GetNodeTypeText(node);
            
            int index = GetIndex(direction);
            
            presentationManager.Enqueue(sequenceId, PresentationPrioirty.NodeSelect_NodeButtonBasePriority + index, InitiatePresentation());
        }

        private string GetMainText(NodeDirection direction) => direction switch
        {
            NodeDirection.Left => LEFT_NODE_MAIN_DESCRIPTION,
            NodeDirection.Middle => MIDDLE_NODE_MAIN_DESCRIPTION,
            NodeDirection.Right => RIGHT_NODE_MAIN_DESCRIPTION,
            _ => throw new InvalidOperationException($"[NextNodeButton] {direction} is not valid")
        };

        private string GetNodeTypeText(Node node) => node switch
        {
            BattleNode battleNode => battleNode.IsBossNode ? "마지막 노드로 이동하기" : "전투 노드로 이동하기",
            IncidentNode => "사건 노드로 이동하기",
            TransactionNode => "거래 노드로 이동하기",
            ScheduleExitNode => "일정 종료 하기",
            _ => throw new InvalidOperationException($"[NextNodeButton] {node.GetType()} is not expected to be a selecting node")
        };

        private int GetIndex(NodeDirection direction) => direction switch
        {
            NodeDirection.Left => 0,
            NodeDirection.Middle => 1,
            NodeDirection.Right => 2,
            _ => throw new InvalidOperationException($"[NextNodeButton] {direction} is not valid")
        };

        public void OnPressed()
        {
            OnNextNodeSelected?.Invoke(node);
        }

        public IEnumerator InitiatePresentation()
        {
            initiateTween?.Kill();
            canvasGroup.interactable = false;

            var startPosition = itemsContainer.sizeDelta;
            itemsContainer.anchoredPosition = new Vector2(startPosition.x, itemsContainer.anchoredPosition.y);
            var targetPosition = Vector2.zero;

            itemsContainer.gameObject.SetActive(true);
            
            initiateTween = itemsContainer.DOAnchorPos(targetPosition, initiateDuration).SetEase(initiateEasingType);
            yield return initiateTween.WaitForCompletion();

            itemsContainer.anchoredPosition = targetPosition;
            canvasGroup.interactable = true;
        }
    }
}