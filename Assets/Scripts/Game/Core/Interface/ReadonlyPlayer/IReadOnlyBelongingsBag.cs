using System;
using System.Collections.Generic;

public interface IReadOnlyBelongingsBag
{
    public IReadOnlyDictionary<BelongingsData, Belongings> MainBelongingsBag { get; }
    public IReadOnlyDictionary<BelongingsData, Belongings> SideBelongingsBag { get; }

    public event Action<IReadOnlyDictionary<BelongingsData, Belongings>> OnMainBagChanged;
    public event Action<IReadOnlyDictionary<BelongingsData, Belongings>> OnSideBagChanged;
}