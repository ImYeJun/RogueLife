using System.Collections.Generic;

public class BattleBelongingsBag : IBattleBelongingsBag
{
    private List<BattleBelongings> belongingsBag = new List<BattleBelongings>(); 

    public List<BattleBelongings> BelongingsBag => belongingsBag;

    public void OnEngageBattle(List<BattleBelongings> belongingsBag, BattleContext context)
    {
        this.belongingsBag = belongingsBag;

        foreach (var belongings in belongingsBag)
        {
            belongings.OnEngageBattle(context);
        }
    }
}