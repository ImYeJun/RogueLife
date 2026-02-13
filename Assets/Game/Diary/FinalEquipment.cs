using System;
using System.Collections.Generic;

[Serializable]
public class FinalEquipment
{
    private Dictionary<CardData, List<Card>> finalMainDeck;
    private Dictionary<CardData, List<Card>> finalSideDeck;
    private Dictionary<BelongingsData, Belongings> finalMainBelongings;
    private Dictionary<BelongingsData, Belongings> finalSideBelongings;

    public FinalEquipment(Dictionary<CardData, List<Card>> finalMainDeck, Dictionary<CardData, List<Card>> finalSideDeck, Dictionary<BelongingsData, Belongings> finalMainBelongings, Dictionary<BelongingsData, Belongings> finalSideBelongings)
    {
        this.finalMainDeck = finalMainDeck;
        this.finalSideDeck = finalSideDeck;
        this.finalMainBelongings = finalMainBelongings;
        this.finalSideBelongings = finalSideBelongings;
    }

    public IReadOnlyDictionary<CardData, List<Card>> FinalMainDeck { get => finalMainDeck; }
    public IReadOnlyDictionary<CardData, List<Card>> FinalSideDeck { get => finalSideDeck; }
    public IReadOnlyDictionary<BelongingsData, Belongings> FinalMainBelongings { get => finalMainBelongings; }
    public IReadOnlyDictionary<BelongingsData, Belongings> FinalSideBelongings { get => finalSideBelongings; }
}