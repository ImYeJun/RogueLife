using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeSkeleton
{
    private Guid id;
    private int layer = 0;
    private NodeType fixedType = NodeType.NONE;
    private List<NodeSkeleton> previousNodes = new List<NodeSkeleton>();
    private List<NodeSkeleton> nextNodes = new List<NodeSkeleton>();
    private int ascendentsWorstBatttleNodeSequence = 0;
    private int ascendentsWorstIncidentNodeSequence = 0;
    private int ascendentsWorstTransactionNodeSequence = 0;

    public NodeSkeleton(int layer, Guid id)
    {
        this.layer = layer;
        this.id = id;
    }

    public NodeSkeleton(int layer, Guid id, NodeType fixedType) : this(layer, id)
    {
        this.fixedType = fixedType;
    }

    public Guid Id { get => id;  }
    public int Layer { get => layer; }
    public NodeType FixedType { get => fixedType; set => fixedType = value; }
    public List<NodeSkeleton> PreviousNodes { get => previousNodes; }
    public List<NodeSkeleton> NextNodes { get => nextNodes; }
    public int AscendentsWorstBatttleNodeSequence { get => ascendentsWorstBatttleNodeSequence; set => ascendentsWorstBatttleNodeSequence = value; }
    public int AscendentsWorstIncidentNodeSequence { get => ascendentsWorstIncidentNodeSequence; set => ascendentsWorstIncidentNodeSequence = value; }
    public int AscendentsWorstTransactionNodeSequence { get => ascendentsWorstTransactionNodeSequence; set => ascendentsWorstTransactionNodeSequence = value; }

    public void InitializeAscenedentSequenceState()
    {
        ascendentsWorstBatttleNodeSequence = 0;
        ascendentsWorstIncidentNodeSequence = 0;
        ascendentsWorstTransactionNodeSequence = 0;
    }

    public void ResetSequenceStateFromPreviousLayer()
    {
        // 타입 초기화
        this.fixedType = NodeType.NONE;
        
        // 시퀀스 카운트 초기화 (0으로 리셋 후 부모들 중 최댓값으로 갱신)
        this.ascendentsWorstBatttleNodeSequence = 0;
        this.ascendentsWorstIncidentNodeSequence = 0;
        this.ascendentsWorstTransactionNodeSequence = 0;

        // 첫 번째 레이어(Layer 0)가 아니라면 부모 노드들을 확인
        if (this.previousNodes != null && this.previousNodes.Count > 0)
        {
            foreach (var parent in this.previousNodes)
            {
                // 부모의 현재 상태를 가져옴 (부모가 BATTLE이면 그 카운트, 아니면 0)
                int pBattle = parent.FixedType == NodeType.BATTLE ? parent.AscendentsWorstBatttleNodeSequence : 0;
                int pIncident = parent.FixedType == NodeType.INCIDENT ? parent.AscendentsWorstIncidentNodeSequence : 0;
                int pTransaction = parent.FixedType == NodeType.TRANSACTION ? parent.AscendentsWorstTransactionNodeSequence : 0;

                // Max값으로 갱신 (RefreshAscenedentSequenceState 로직과 동일)
                this.ascendentsWorstBatttleNodeSequence = Mathf.Max(this.ascendentsWorstBatttleNodeSequence, pBattle);
                this.ascendentsWorstIncidentNodeSequence = Mathf.Max(this.ascendentsWorstIncidentNodeSequence, pIncident);
                this.ascendentsWorstTransactionNodeSequence = Mathf.Max(this.ascendentsWorstTransactionNodeSequence, pTransaction);
            }
        }
    }

    public bool IsAscendentsSequenceValid(ScheduleNodeTypeResolveRule rule)
    {
        return 
            ascendentsWorstBatttleNodeSequence <= rule.MaxBattleSequence &&
            ascendentsWorstIncidentNodeSequence <= rule.MaxIncidentSequence &&
            ascendentsWorstTransactionNodeSequence <= rule.MaxTransactionSequence;
    }
}