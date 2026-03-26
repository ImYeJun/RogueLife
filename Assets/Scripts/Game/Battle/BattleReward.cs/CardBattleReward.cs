using TMPro;
using UnityEngine;

public class CardBattleReward : IBattleReward
{
    private Card card;

    public CardBattleReward(Card card)
    {
        this.card = card;
    }

    public string Description => $"{card.CurrentName} (카드)";

    public void Resolve(IScheduleViewCommander commander)
    {
        commander.ObtainCard(card);
    }
}