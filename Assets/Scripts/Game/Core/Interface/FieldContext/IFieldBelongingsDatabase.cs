#nullable enable

using System.Collections.Generic;

public interface IFieldBelongingsDatabase
{
    Belongings? GetRandomBelongings(System.Random random, List<BelongingsData>? ignoring = null);
    public Belongings? Materialize(BelongingsData belongingsData);
}