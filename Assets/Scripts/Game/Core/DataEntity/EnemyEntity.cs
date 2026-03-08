using UnityEngine;

public class EnemyEntity : MonoBehaviour 
{
    [SerializeField] private EnemyData data;
    [SerializeReference, SubclassSelector] private BattleEnemyBehaviour battleBehaviour;
    
    public EnemyData Data => data;
    public string Id => data.Id;
    public string EnemyName => data.EnemyName;
    public EnemyTier Tier => data.Tier;
    public string Description => data.Description;
    public int MaxBaseHealth => data.MaxBaseHealth;
    public Sprite UsualSprite => data.UsualSprite;
    public Sprite BattleIdleSprite => data.BattleIdleSprite;
    public Sprite BattleActionSprite => data.BattleActionSprite;

    public BattleEnemyBehaviour CloneBehaviour(IEnemyBehaviourOwner owner)
    {
        if (battleBehaviour == null)
        {
            Debug.LogError($"[EnemyEntity] Behaviour is missing in {EnemyName} ({Id})");
            return null;
        }

        return battleBehaviour.Clone(owner);
    }
}