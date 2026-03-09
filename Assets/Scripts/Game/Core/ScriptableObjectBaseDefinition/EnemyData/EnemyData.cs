#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Serializable]
    public struct EnemySpriteMapping
    {
        [SerializeField] private EnemySpriteType type;
        [SerializeField] private Sprite sprite;

        public EnemySpriteType Type => type;
        public Sprite Sprite => sprite;
    }

    [Header("Basic Information")]
    [SerializeField] protected string id;
    [SerializeField] protected string enemyName;
    [SerializeField] protected EnemyTier tier;
    [SerializeField, TextArea] protected string description;
    [SerializeField] protected int maxBaseHealth;

    [Header("Dialogue")]
    [SerializeField, TextArea] private List<string> encounterLines;
    [SerializeField, TextArea] private List<string> actionLines;
    [SerializeField, TextArea] private List<string> victoryLines;
    [SerializeField, TextArea] private List<string> defeatLines;

    [Header("Visuals")]
    [SerializeField] protected Sprite usualSprite; 
    [SerializeField] protected List<EnemySpriteMapping> battleSprites; 

    public string Id => id;
    public string EnemyName => enemyName;
    public string Description => description;
    public int MaxBaseHealth => maxBaseHealth;
    public EnemyTier Tier => tier;
    
    public IReadOnlyList<string> EncounterLines => encounterLines;
    public IReadOnlyList<string> ActionLines => actionLines;
    public IReadOnlyList<string> VictoryLines => victoryLines;
    public IReadOnlyList<string> DefeatLines => defeatLines;

    public Sprite UsualSprite => usualSprite;

    public Sprite? GetBattleSprite(EnemySpriteType type) 
        => battleSprites.FirstOrDefault(item => item.Type == type).Sprite;
    public Sprite? BattleIdleSprite => GetBattleSprite(EnemySpriteType.Idle);
    public Sprite? BattleActionSprite => GetBattleSprite(EnemySpriteType.Action);
}