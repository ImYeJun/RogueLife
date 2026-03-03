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
    [Header("Test")]
    [SerializeField] private List<BelongingsData> belongingsData;

    [ContextMenu("AddTestBelongings")]
    public void AddTestBelongings()
    {
        foreach (var data in belongingsData)
        {
            CurrentRun?.TestAddBelongigns(data);
        }
    }
#endif
}