using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBelongingsBag : IFieldBelongingsBag, IRunDiaryPlayerBelongingsBag
{
    Dictionary<BelongingsData, Belongings> mainBelongingsBag = new Dictionary<BelongingsData, Belongings>();
    Dictionary<BelongingsData, Belongings> sideBelongingsBag = new Dictionary<BelongingsData, Belongings>();
    private FieldContext context;

    public event Action<IReadOnlyDictionary<BelongingsData, Belongings>> OnMainBagChanged;
    public event Action<IReadOnlyDictionary<BelongingsData, Belongings>> OnSideBagChanged;

    public IReadOnlyDictionary<BelongingsData, Belongings> MainBelongingsBag { get => mainBelongingsBag; }
    public IReadOnlyDictionary<BelongingsData, Belongings> SideBelongingsBag { get => sideBelongingsBag; }

    public List<Belongings> EquippingBelongings => mainBelongingsBag.Values.Concat(sideBelongingsBag.Values).ToList();

    public List<BelongingsData> GetClonedMainBag() { 
        var result = new List<BelongingsData>();

        foreach (var pair in mainBelongingsBag)
        {
            result.Add(pair.Key);
        }

        return result;
    }

    public List<BelongingsData> GetClonedSideBag() { 
        var result = new List<BelongingsData>();

        foreach (var pair in sideBelongingsBag)
        {
            result.Add(pair.Key);
        }

        return result;
    }

    public void InitializeContext(FieldContext context)
    {
        this.context = context;
    }

    public List<BattleBelongings> GetBattleBelongings(IBattleBelongingsOwner owner)
    {
        var result = new List<BattleBelongings>();

        foreach(var belongings in mainBelongingsBag.Values)
        {
            result.Add(belongings.GenerateBattleBelongings(owner));
        }

        return result;
    }

    public bool TryObtainBelongings(Belongings belongings)
    {
        if (HasBelongings(belongings))
        {
            Debug.Log($"[PlayerBelongingsBag] Player already has {belongings.Name}");
            return false;
        }

        sideBelongingsBag[belongings.Data] = belongings;
        return true;
    }

    public bool TryMoveBelongings(Belongings belongings, BelongingsBagType from, BelongingsBagType to)
    {
        if (from == to)
        {
            Debug.Log($"[PlayerBelongingsBag] The arguments 'from' and 'to' cannot be the same.");
            return false;
        }

        if (!HasBelongings(belongings, from))
        {
            Debug.Log($"[PlayerBelongingsBag] There is no {belongings.Name} Belongings in {from}");
            return false;
        }
        if (HasBelongings(belongings, to))
        {
            Debug.Log($"[PlayerBelongingsBag] {to} already has {belongings.Name}");
            return false;
        }  
        if (to == BelongingsBagType.MAIN_BELONGINGS_BAG && mainBelongingsBag.Count >= Constant.MAX_MAIN_BELONINGS_COUNT)
        {
            Debug.Log("[PlayerBelongingsBag] Main Belongings Bag is full");
            return false;
        }

        if (context is null)
        {
            throw new InvalidOperationException("[PlayerBelongingsBag] context hasn't been initalized");
        }

        if (from == BelongingsBagType.MAIN_BELONGINGS_BAG) belongings.OnUnequipped(context);
        if (to == BelongingsBagType.MAIN_BELONGINGS_BAG) belongings.OnEquipped(context);

        GetBag(from).Remove(belongings.Data);
        GetBag(to)[belongings.Data] = belongings;

        return true;
    }

    public bool HasBelongings(Belongings belongings, BelongingsBagType bagType)
    {
        Dictionary<BelongingsData, Belongings> bag = GetBag(bagType);
        
        return bag.ContainsKey(belongings.Data);
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
            _ => throw new ArgumentOutOfRangeException($"[PlayerBelongingsBag] {bagType} is not valid.")
        };
    }
}
