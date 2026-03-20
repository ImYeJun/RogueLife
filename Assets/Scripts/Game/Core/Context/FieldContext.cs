using System;
using System.Collections.Generic;

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
    private IFieldSchedule schedule;

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

    public void SetScehdule(IFieldSchedule schedule)
    {
        this.schedule = schedule;
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
    public IFieldSchedule Schedule { get => schedule;  }
}