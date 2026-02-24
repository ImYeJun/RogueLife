using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected string enemyName;
    [SerializeField] protected EnemyTier tier;
    [SerializeField, TextArea] protected string description;
    [SerializeField] protected int maxBaseHealth;
    [SerializeField, TextArea] private List<string> encounterLines;
    [SerializeField, TextArea] private List<string> actionLines;
    [SerializeField, TextArea] private List<string> victoryLines;
    [SerializeField, TextArea] private List<string> defeatLines;
    [SerializeField] protected Sprite usualSprite;
    [SerializeField] protected Sprite battleSprite;
    [SerializeReference, SubclassSelector] protected BattleEnemyBehaviour battleBehaviour;

    public string Id { get => id; }
    public string EnemyName { get => enemyName; }
    public string Description { get => description; }
    public int MaxBaseHealth { get => maxBaseHealth; }
    protected List<string> EncounterLines { get => encounterLines; }
    protected List<string> ActionLines { get => actionLines; }
    protected List<string> VictoryLines { get => victoryLines; }
    protected List<string> DefeatLines { get => defeatLines; }
    public Sprite UsualSprite { get => usualSprite; }
    public Sprite BattleSprite { get => battleSprite; }
    public EnemyTier Tier { get => tier; }

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