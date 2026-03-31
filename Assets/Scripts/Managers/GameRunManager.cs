#nullable enable

using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

public partial class GameRunManager : SingletonManager<GameRunManager>
{
    public GameRun? CurrentRun { get; private set; }
    
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private SkeletonGenerateRuleManager skeletonGenerateRuleManager;
    [SerializeField] private StartDeck startDeckForEmptyRun;
    

    public void StartNewRun(StartDeck startDeck)
    {
        var databases = databaseManager.Databaes;
        var rules = skeletonGenerateRuleManager.Rules;

        CurrentRun = new GameRun(rules, databases, startDeck, OnRunEnded);
        CurrentRun.StartGame();
    }

    public GameRun GetEmptyRun()
    {
        var databases = databaseManager.Databaes;
        var rules = skeletonGenerateRuleManager.Rules;

        return new GameRun(rules, databases, startDeckForEmptyRun, OnRunEnded);
    }

    public void OnRunEnded()
    {
        CurrentRun = null;
    }
}