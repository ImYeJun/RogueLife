using System;
using System.Collections.Generic;

public interface IReadOnlyBelongingsBag
{
    public IReadOnlyDictionary<BelongingsData, Belongings> MainBelongingsBag { get; }
    public IReadOnlyDictionary<BelongingsData, Belongings> SideBelongingsBag { get; }
}