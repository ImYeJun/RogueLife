using System;

public class FieldContext
{
    private Random random;
    private IFieldTransactionChoiceDatabase transactionChoiceDatabase;
    private IFieldCardDatabase cardDatabase;
    private IFieldBelongingsDatabase belongingsDatabase;
    private IFieldScheduleSystem scheduleSystem;
    private IFieldBattleSystem battleSystem;
    private IFieldHealth health;
    private IFieldActionCost actionCost;
    private IFieldDeck deck;
    private IFieldBelongingsBag belongingsBag;

    public FieldContext(Random random, IFieldTransactionChoiceDatabase transactionChoiceDatabase, IFieldCardDatabase cardDatabase, IFieldBelongingsDatabase belongingsDatabase, IFieldScheduleSystem scheduleSystem, IFieldBattleSystem battleSystem, IFieldHealth health, IFieldActionCost actionCost, IFieldDeck deck, IFieldBelongingsBag belongingsBag)
    {
        this.random = random;
        this.transactionChoiceDatabase = transactionChoiceDatabase;
        this.cardDatabase = cardDatabase;
        this.belongingsDatabase = belongingsDatabase;
        this.scheduleSystem = scheduleSystem;
        this.battleSystem = battleSystem;
        this.health = health;
        this.actionCost = actionCost;
        this.deck = deck;
        this.belongingsBag = belongingsBag;
    }

    public Random Random { get => random; }
    public IFieldTransactionChoiceDatabase TransactionChoiceDatabase { get => transactionChoiceDatabase; }
    public IFieldCardDatabase CardDatabase { get => cardDatabase; }
    public IFieldBelongingsDatabase BelongingsDatabase { get => belongingsDatabase; }
    public IFieldScheduleSystem ScheduleSystem { get => scheduleSystem; }
    public IFieldBattleSystem BattleSystem { get => battleSystem; }
    public IFieldHealth Health { get => health; }
    public IFieldActionCost ActionCost { get => actionCost; }
    public IFieldDeck Deck { get => deck; }
    public IFieldBelongingsBag BelongingsBag { get => belongingsBag; }
    
    //! This code is a total hack! It's completely fucked up!
    //! To avoid side effects, this MUST be used ONLY for implementing the 'ChoiceEngageBattleEffect' feature.
    //! Technically, the right way is to refactor the BattleSystem structure. 
    //! But I have to create over 200 data entries immediately, so there's absolutely no time.
    //! I had no choice but to use this hack. Shit!!!!!!
    //! I'll refactor this shitty code if I ever get some spare time... or when this code inevitably blows up.
    public bool HasEngagedBattleByChoiceEngageBattleEffect { get; set; } = false;
    public Action RequestNextNodeSelectionForChoiceEngageBattleEffect;
    public Action<EnemyData, bool> RecordEncounterEnemyForChoiceEngageBattleEffect;
    public Action OnPlayerMentalBrokenForChoiceEngageBattleEffect;
}