public class BattleDeckSystem : IBattleDeckSystemContext, IBattleEventObserver
{
    private HandDeck handDeck;
    private DrawDeck drawDeck;
    private GraveDeck graveDeck;
    private CardPlayHistory history;

    public void MoveCard(Card card, BattleDeckType destination)
    {
        
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        throw new System.NotImplementedException();
    }
}