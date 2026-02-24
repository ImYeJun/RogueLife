using System.Collections.Generic;

public interface IFieldCardDatabase : IBattleCardDatabase
{
    public Card Materialize(CardData data);
}