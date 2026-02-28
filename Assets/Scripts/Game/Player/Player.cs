public class Player
{
    public Player(StartDeck startDeck, CardDatabase cardDatabase)
    {
        Health = new PlayerHealth();
        ActionCost = new PlayerActionCost();
        Deck = new PlayerDeck(startDeck, cardDatabase);
        BelongingsBag = new PlayerBelongingsBag();
    }

    public PlayerHealth Health { get; private set;}
    public PlayerActionCost ActionCost { get; private set;}
    public PlayerDeck Deck { get; }
    public PlayerBelongingsBag BelongingsBag { get; private set;}
}