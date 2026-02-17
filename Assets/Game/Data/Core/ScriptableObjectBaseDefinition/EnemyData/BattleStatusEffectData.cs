using UnityEngine;

[CreateAssetMenu(fileName = "BattleStatusEffectData", menuName = "Scriptable Objects/BattleStatusEffectData")]
public class BattleStatusEffectData : ScriptableObject {
    [SerializeField] private string id;
    [SerializeField] private string battleStatusEffectName;
    [SerializeField] private string description;
    [SerializeField] private BattleStatusEffectType type;
    [SerializeField] private BattleEntityCondition grantedCondition;
    [SerializeField] private BattleEntityTrait requiredTraits;
    [SerializeField] private Sprite icon;
    [SerializeReference, SubclassSelector] private BattleStatusEffectBehaviour behaviour;

    public string Id { get => id; }
    public string Name { get => battleStatusEffectName; set => battleStatusEffectName = value; }
    public string Description { get => description; }
    public BattleStatusEffectType Type { get => type; }
    public BattleEntityCondition GrantedCondition { get => grantedCondition; }
    public BattleEntityTrait RequiredTraits { get => requiredTraits; }
    public Sprite Icon { get => icon; }

    public BattleStatusEffectBehaviour CloneBehaviour(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
    {
        return behaviour.Clone(context, owner, state);
    }
}