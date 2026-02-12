using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string enemyName;
    [SerializeField] private string description;
    [SerializeField] private int maxBaseHealth;
    [SerializeField] private string encounterLine;
    [SerializeField] private string victoryLine;
    [SerializeField] private string defeatLine;
    [SerializeField] private EnemyTier tier;
    [SerializeField] private Sprite usualSprite;
    [SerializeField] private Sprite battleSprite;
    [SerializeReference, SubclassSelector] private BattleEnemyBehaviour battleBehaviour;

    public string EnemyName { get => enemyName; }
    public string Description { get => description; }
    public int MaxBaseHealth { get => maxBaseHealth; }
    public string EncounterLine { get => encounterLine; }
    public string VictoryLine { get => victoryLine; }
    public string DefeatLine { get => defeatLine; }
    public EnemyTier Tier { get => tier; }
    public Sprite UsualSprite { get => usualSprite; }
    public Sprite BattleSprite { get => battleSprite; }

    public BattleEnemyBehaviour CloneBehaviour()
    {
        return battleBehaviour.Clone();
    }
}