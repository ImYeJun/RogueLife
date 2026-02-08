public interface IFieldCardDatabase
{
    public Card GetRandomCard(CardType type, CardAttribute attribute);
    public Card MaterializeCardData(CardData data);
}