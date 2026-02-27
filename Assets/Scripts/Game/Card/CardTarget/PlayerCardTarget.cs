public class PlayerCardTarget : CardTarget
{
    private BattlePlayer player;

    public PlayerCardTarget(BattlePlayer player)
    {
        this.player = player;
    }

    public BattlePlayer Player { get => player; }
}