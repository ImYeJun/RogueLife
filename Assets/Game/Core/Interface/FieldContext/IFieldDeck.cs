using System.Collections.Generic;
using Field.Deck.Observers;

public interface IFieldDeck : IBattleEntryDeck {
    public void RegisterDeckobserver(IDeckObserver observer);
    public void UnrgisterDeckobserver(IDeckObserver observer);
    public bool HasEnoughCard(CardData data, int amount = 1);
    public List<Card> GetSpecificCardsByData(CardData data);
    public bool TryObtainCard(Card card);
    public bool TryRemoveRandomCard(System.Random random, CardType type, CardAttribute attribute);
    public bool TryRemoveCardByData(CardData data, int amount);
    public void IncreaseMaxCardVariety(int amount);
    public void DecreaseMaxCardVariety(int amount);
}