using System.Collections.Generic;

public interface IBattleEntryBelongingsBag
{
    public List<BattleBelongings> GetBattleBelongings(IBattleBelongingsOwner owner);
}