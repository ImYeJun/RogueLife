using UnityEngine;

public class BattleStatusEffectEntity : MonoBehaviour {
    [SerializeField] private BattleStatusEffectData data;
    [SerializeReference, SubclassSelector] private BattleStatusEffectBehaviour behaviour;

    public string Id { get => data.Id; }
    public BattleStatusEffectData Data { get => data; }

    public BattleStatusEffectBehaviour CloneBehaviour(BattleContext context, IBattleStatusEffectOwner owner, IBattleStatusEffectState state)
    {
        return behaviour.Clone(context, owner, state);
    }
}