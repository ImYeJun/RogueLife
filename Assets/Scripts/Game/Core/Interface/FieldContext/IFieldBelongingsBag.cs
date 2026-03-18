using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFieldBelongingsBag : IBattleEntryBelongingsBag, IReadOnlyBelongingsBag {
    public bool TryObtainBelongings(Belongings belongings);
    public bool TryMoveBelongings(Belongings belongings, BelongingsBagType from, BelongingsBagType to);
    public List<Belongings> EquippingBelongings { get; }
    public event Action<Belongings> OnBelongingsObtained;
}