using System;
using UnityEngine;

namespace View.ScheduleView.NextNodeSelectView
{
    public class NextNodeButton : DoubleTextSelectButton
    {
        public enum NodeDirection { Left = 0, Middle = 1, Right = 2 }
        
        private const string LEFT_NODE_MAIN_DESCRIPTION = "왼쪽으로 가기";
        private const string MIDDLE_NODE_MAIN_DESCRIPTION = "직진하기";
        private const string RIGHT_NODE_MAIN_DESCRIPTION = "오른쪽으로 가기";

        public void Initiate(NodeDirection direction, Node node, Action<Node> onNextNodeSelected)
        {
            Initialize(() => onNextNodeSelected?.Invoke(node), GetMainText(direction), GetNodeTypeText(node));
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
    }
}