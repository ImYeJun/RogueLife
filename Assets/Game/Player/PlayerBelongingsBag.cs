using System.Collections.Generic;
using UnityEngine;

public class PlayerBelongingsBag
{
    Dictionary<BelongingsData, Belongings> mainBelongingsBag = new Dictionary<BelongingsData, Belongings>();
    Dictionary<BelongingsData, Belongings> sideBelongingsBag = new Dictionary<BelongingsData, Belongings>();

    public IReadOnlyDictionary<BelongingsData, Belongings> MainBelongingsBag { get => mainBelongingsBag; }
    public IReadOnlyDictionary<BelongingsData, Belongings> SideBelongingsBag { get => sideBelongingsBag; }

    public bool TryObtainBelongings(Belongings belongings)
    {
        if (HasBelongings(belongings))
        {
            Debug.Log($"Player already has {belongings.Name}");
            return false;
        }

        sideBelongingsBag[belongings.Data] = belongings;
        return true;
    }

    public bool TryMoveBelongings(Belongings belongings, BelongingsBagType from, BelongingsBagType to)
    {
        if (from == to)
        {
            Debug.Log($"The arguments 'from' and 'to' cannot be the same.");
            return false;
        }

        if (!HasBelongings(belongings, from))
        {
            Debug.Log($"There is no {belongings.Name} Belongings in {from}");
            return false;
        }
        if (HasBelongings(belongings, to))
        {
            Debug.Log($"{to} already has {belongings.Name}");
            return false;
        }  

        GetBag(from).Remove(belongings.Data);
        GetBag(to)[belongings.Data] = belongings;

        return true;
    }

    public bool HasBelongings(Belongings belongings, BelongingsBagType bagType)
    {
        Dictionary<BelongingsData, Belongings> bag = GetBag(bagType);

        if (bag == null) { return false;}
        if (!bag.ContainsKey(belongings.Data)) { return false; }

        return bag[belongings.Data] == belongings;
    }

    public bool HasBelongings(Belongings belongings)
    {
        return HasBelongings(belongings, BelongingsBagType.MAIN_BELONGINGS_BAG) || HasBelongings(belongings, BelongingsBagType.SIDE_BELONGINGS_BAG);
    }

    private Dictionary<BelongingsData, Belongings> GetBag(BelongingsBagType bagType)
    {
        return bagType switch
        {
            BelongingsBagType.MAIN_BELONGINGS_BAG => mainBelongingsBag,
            BelongingsBagType.SIDE_BELONGINGS_BAG => sideBelongingsBag,
            _ => null
        };
    }
}
