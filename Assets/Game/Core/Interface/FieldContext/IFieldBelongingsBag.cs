using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFieldBelongingsBag : IBattleEntryBelongingsBag {
    public bool TryObtainBelongings(Belongings belongings);
    public IReadOnlyDictionary<BelongingsData, Belongings> MainBelongingsBag { get; }
    public List<BelongingsData> EquippingBelongings { get; }
}