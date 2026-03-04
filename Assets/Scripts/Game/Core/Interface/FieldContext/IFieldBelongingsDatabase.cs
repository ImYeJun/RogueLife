#nullable enable

using System.Collections.Generic;

public interface IFieldBelongingsDatabase
{
    Belongings? GetRandomBelongings(System.Random random, List<Belongings>? ignoring = null);
    public Belongings? Materialize(BelongingsEntity belongingsData);
}