using System.Collections.Generic;

public interface IRunDiaryPlayerBelongingsBag
{
    public Dictionary<BelongingsData, Belongings> GetClonedMainBag();
    public Dictionary<BelongingsData, Belongings> GetClonedSideBag();
}