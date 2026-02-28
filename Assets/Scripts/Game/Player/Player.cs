public class Player
{
    public PlayerHealth Health { get; } = new PlayerHealth();
    public PlayerActionCost ActionCost { get; } = new PlayerActionCost();
    public PlayerDeck Deck { get; } = new PlayerDeck();
    public PlayerBelongingsBag BelongingsBag { get; } = new PlayerBelongingsBag();
}