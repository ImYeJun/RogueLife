using Battle.HurtSources;

public class DirectlyDecreaseMentalityBattleAction : IBattleAction
{
    private int amount;

    public DirectlyDecreaseMentalityBattleAction(int amount)
    {
        this.amount = amount;
    }

    public void Execute(BattleContext context)
    {
        context.PlayerContainer.Player.HurtMentality(amount);
    }
}