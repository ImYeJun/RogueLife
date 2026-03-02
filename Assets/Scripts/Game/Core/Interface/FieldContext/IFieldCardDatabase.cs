#nullable enable

using System.Collections.Generic;

public interface IFieldCardDatabase : IBattleCardDatabase
{
    public Card? Materialize(CardEntity entity);
    public Card? Materialize(string id);
}