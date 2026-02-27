using System.Collections.Generic;

public interface IRunDiaryPlayerBelongingsBag
{
    public List<BelongingsData> GetClonedMainBag();
    public List<BelongingsData> GetClonedSideBag();
}