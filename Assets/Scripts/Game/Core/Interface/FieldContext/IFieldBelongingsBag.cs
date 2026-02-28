using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFieldBelongingsBag : IBattleEntryBelongingsBag, IReadOnlyBelongingsBag {
    public bool TryObtainBelongings(Belongings belongings);
    public List<BelongingsData> EquippingBelongings { get; }
}