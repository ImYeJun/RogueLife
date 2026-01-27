public class ChoiceContext
{
    private IChoiceScheduleSystem scheduleSystem;
    private BattleSystem battleSystem;
    private IChoiceHealth health;
    private IChoiceActionCost actionCost;
    private IChoiceDeck deck;
    private IChoiceBelongingsBag belongingsBag;

    public IChoiceScheduleSystem ScheduleSystem { get => scheduleSystem;}
    public BattleSystem BattleSystem { get => battleSystem;}
    public IChoiceHealth Health { get => health;}
    public IChoiceActionCost ActionCost { get => actionCost;}
    public IChoiceDeck Deck { get => deck;}
    public IChoiceBelongingsBag BelongingsBag { get => belongingsBag;}
}