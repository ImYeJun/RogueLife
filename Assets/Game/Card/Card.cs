using UnityEngine;

public class Card
{
    private CardData data;
    private string currentName;
    private string currentDescription;
    private CardType currentType;
    private CardAttribute currentAttribute;
    private CardRarity currentRarity;
    private int currentActionCost;
    private bool isReflectionApplied = false;

    public CardData Data { get => data; }
    public string CurrentName { get => currentName; }
    public string CurrentDescription { get => currentDescription; }
    public CardType CurrentType { get => currentType; }
    public CardAttribute CurrentAttribute { get => currentAttribute; }
    public CardRarity CurrentRarity { get => currentRarity; }
    public int CurrentActionCost { get => currentActionCost; }
    public bool IsReflectionApplied { get => isReflectionApplied; }

    public Card(Card card)
    {
        data = card.Data;
        currentName = card.CurrentName;
        currentDescription = card.CurrentDescription;
        currentType = card.CurrentType;
        currentAttribute = card.CurrentAttribute;
        currentRarity = card.CurrentRarity;
        currentActionCost = card.CurrentActionCost;
        isReflectionApplied = card.IsReflectionApplied;
    }

    public void Execute(BattleContext context)
    {
        if (isReflectionApplied) { data.ExecuteReflection(context); }
        else { data.Execute(context); }
    }

    public void ApplyReflection() { isReflectionApplied = true; }
    public void UnapplyReflection() { isReflectionApplied = false; }

    public void Equals(Card operand) => operand.Data.Equals(data);
}