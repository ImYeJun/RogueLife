using System;
using TMPro;
using UnityEngine;

namespace View.ScheduleView.NextNodeSelectView
{
    public class NextNodeButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI subText;

        private Node node;
        private Action<Node> OnNextNodeSelected;

        public void Initiate(string mainDescription, Node node, Action<Node> onNextNodeSelected)
        {
            this.node = node;
            OnNextNodeSelected = onNextNodeSelected;

            mainText.text = mainDescription;

            string nodeTypeText = node switch
            {
                BattleNode battleNode => battleNode.IsBossNode ? "마지막 노드로 이동하기" : "전투 노드로 이동하기",
                IncidentNode => "사건 노드로 이동하기",
                TransactionNode => "거래 노드로 이동하기",
                ScheduleExitNode => "일정 종료 하기",
                _ => throw new InvalidOperationException($"[NextNodeButton] {node.GetType()} is not expected to be a selecting node")
            };

            subText.text = $"{nodeTypeText}";
        }

        public void OnPressed()
        {
            if (OnNextNodeSelected == null) return;

            var actionToInvoke = OnNextNodeSelected;
            OnNextNodeSelected = null;
            actionToInvoke.Invoke(node);
        }
    }
}