using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected string enemyName;
    [SerializeField] protected string description;
    [SerializeField] protected int maxBaseHealth;
    [SerializeField] protected string encounterLine;
    [SerializeField] protected string victoryLine;
    [SerializeField] protected string defeatLine;
    [SerializeField] protected Sprite usualSprite;
    [SerializeField] protected Sprite battleSprite;
    [SerializeReference, SubclassSelector] protected BattleEnemyBehaviour battleBehaviour;
    protected EnemyTier tier;
    protected int lossMentalityOnUnresolved;
    protected EnemyResolveReward reward;

    public string Id { get => id; }
    public string EnemyName { get => enemyName; }
    public string Description { get => description; }
    public int MaxBaseHealth { get => maxBaseHealth; }
    public string EncounterLine { get => encounterLine; }
    public string VictoryLine { get => victoryLine; }
    public string DefeatLine { get => defeatLine; }
    public Sprite UsualSprite { get => usualSprite; }
    public Sprite BattleSprite { get => battleSprite; }
    public EnemyTier Tier { get => tier; }
    public int LossMentalityOnUnresolved { get => lossMentalityOnUnresolved; }
    public EnemyResolveReward Reward { get => reward; }

    public BattleEnemyBehaviour CloneBehaviour(IEnemyBehaviourOwner owner)
    {
        if (battleBehaviour == null)
        {
            Debug.LogError($"[EnemyData] Behaviour is missing in {enemyName} ({id})");
        return null;
        }

        return battleBehaviour.Clone(owner);
    }
}