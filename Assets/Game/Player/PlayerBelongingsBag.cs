using System.Collections.Generic;
using UnityEngine;

public class PlayerBelongingsBag
{
    List<Belongings> sideBelongingsBag = new List<Belongings>();
    List<Belongings> mainBelongingsBag = new List<Belongings>();

    public IReadOnlyList<Belongings> SideBelongingsBag { get => sideBelongingsBag.AsReadOnly(); }
    public IReadOnlyList<Belongings> MainBelongingsBag { get => mainBelongingsBag.AsReadOnly(); }

    public bool TryObtainBelongings(Belongings belongings)
    {
        if (HasBelongings(belongings, sideBelongingsBag) || HasBelongings(belongings, mainBelongingsBag))
        {
            Debug.Log($"Player already has {belongings.Name}");
            return false;
        }

        sideBelongingsBag.Add(belongings);
        return true;
    }

    public bool TryMoveBelongings(Belongings belongings, BelongingsBagType from, BelongingsBagType to)
    {
        if (from == to)
        {
            Debug.Log($"The arguments 'from' and 'to' cannot be the same.");
            return false;
        }

        List<Belongings> fromBag = GetBag(from);
        List<Belongings> toBag = GetBag(to);

        if (fromBag == null || toBag == null)
        {
            Debug.LogError($"The BelongingsBagTypes {from} or {to} are not valid.");
            return false;
        }
        if (!HasBelongings(belongings, fromBag))
        {
            Debug.Log($"There is no {belongings.Name} Belongings in {from}");
            return false;
        }
        if (HasBelongings(belongings, toBag))
        {
            Debug.Log($"{to} already has {belongings.Name}");
            return false;
        }  

        fromBag.Remove(belongings);
        toBag.Add(belongings);

        return true;
    }

    private bool HasBelongings(Belongings belongings, List<Belongings> bag)
    {
        foreach (Belongings equippedBelongings in bag)
        {
            if (equippedBelongings.Equals(belongings)) return true;
        }

        return false;
    }

    private List<Belongings> GetBag(BelongingsBagType type)
    {
        return type switch
        {
            BelongingsBagType.MAIN_BELONGINGS_BAG => mainBelongingsBag,
            BelongingsBagType.SIDE_BELONGINGS_BAG => sideBelongingsBag,
            _ => null
        };
    }
}
