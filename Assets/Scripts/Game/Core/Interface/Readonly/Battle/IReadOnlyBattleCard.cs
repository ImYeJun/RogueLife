using UnityEngine;

public interface IReadOnlyBattleCard {
    public Sprite Background { get; }
    public string CurrentName { get; }
    public string CurrentDescription { get; }
    public string NormalEffectDescription { get; }
    public string ReflectionEffectDescription { get; }
    public CardType CurrentType { get; }
    public CardAttribute CurrentAttribute { get; }
    public CardRarity CurrentRarity { get; }
    public bool IsReflectionApplied { get; }
    public CardTargetType TargetType { get; }
    public int CurrentActionCost { get; }
}