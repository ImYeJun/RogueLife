#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

public class GameRunManager : SingletonManager<GameRunManager>
{
    public GameRun? CurrentRun { get; private set; }
    
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private SkeletonGenerateRuleManager skeletonGenerateRuleManager;

    public void StartNewRun(StartDeck startDeck)
    {
        var databases = databaseManager.Databaes;
        var rules = skeletonGenerateRuleManager.Rules;

        CurrentRun = new GameRun(rules, databases, startDeck);
        CurrentRun.StartGame();
    }

//* Test Code
#if UNITY_EDITOR
    [Space(20)]
    [Header("=== DEBUG / TEST TOOLS ===")]
    [Tooltip("테스트 시에만 사용하는 데이터 리스트입니다.")]
    [SerializeField] private List<BelongingsEntity> testBelongingsEntities;

    [ContextMenu("DEBUG: Add Test Belongings")] // 이름을 명확히 변경
    public void AddTestBelongings()
    {
        if (CurrentRun == null) {
            Debug.LogWarning("게임이 실행 중이 아닙니다.");
            return;
        }

        foreach (var data in testBelongingsEntities)
        {
            CurrentRun.TestAddBelongings(data);
        }
    }
#endif
}