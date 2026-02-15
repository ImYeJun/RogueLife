using System;

public class FieldContext
{
    private Random random;
    private IFieldCardDatabase cardDatabase;
    private IFieldTransactionChoiceDatabase transactionChoiceDatabase;
    private IFieldDeck deck;
    private IFieldBelongingsDatabase belongingsDatabase;
    private IFieldBelongingsBag belongingsBag;
    private IFieldScheduleSystem scheduleSystem;
    private IFieldBattleSystem battleSystem;
    private IFieldActionCost actionCost;
    private IFieldHealth health;

    public FieldContext(Random random, IFieldCardDatabase cardDatabase, IFieldTransactionChoiceDatabase transactionChoiceDatabase, IFieldDeck deck, IFieldBelongingsDatabase belongingsDatabase, IFieldBelongingsBag belongingsBag, IFieldScheduleSystem scheduleSystem, IFieldBattleSystem battleSystem, IFieldActionCost actionCost, IFieldHealth health)
    {
        this.random = random;
        this.cardDatabase = cardDatabase;
        this.transactionChoiceDatabase = transactionChoiceDatabase;
        this.deck = deck;
        this.belongingsDatabase = belongingsDatabase;
        this.belongingsBag = belongingsBag;
        this.scheduleSystem = scheduleSystem;
        this.battleSystem = battleSystem;
        this.actionCost = actionCost;
        this.health = health;
    }

    public Random Random { get => random; }
    public IFieldCardDatabase CardDatabase { get => cardDatabase; }
    public IFieldTransactionChoiceDatabase TransactionChoiceDatabase { get => transactionChoiceDatabase;  }
    public IFieldDeck Deck { get => deck;}
    public IFieldBelongingsDatabase BelongingsDatabase { get => belongingsDatabase; }
    public IFieldBelongingsBag BelongingsBag { get => belongingsBag;}
    public IFieldScheduleSystem ScheduleSystem { get => scheduleSystem;}
    public IFieldBattleSystem BattleSystem { get => battleSystem;}
    public IFieldActionCost ActionCost { get => actionCost;}
    public IFieldHealth Health { get => health;}
}