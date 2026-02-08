using System;

public class FieldContext
{
    private Random random;
    private IFieldCardDatabase cardDatabase;
    private IFieldDeck deck;
    private IFieldBelongingsDatabase belongingsDatabase;
    private IFieldBelongingsBag belongingsBag;
    private IFieldScheduleSystem scheduleSystem;
    private IFieldBattleSystem battleSystem;
    private IFieldActionCost actionCost;
    private IFieldHealth health;

    public Random Random { get => random; }
    public IFieldCardDatabase CardDatabase { get => cardDatabase; }
    public IFieldDeck Deck { get => deck;}
    public IFieldBelongingsDatabase BelongingsDatabase { get => belongingsDatabase; }
    public IFieldBelongingsBag BelongingsBag { get => belongingsBag;}
    public IFieldScheduleSystem ScheduleSystem { get => scheduleSystem;}
    public IFieldBattleSystem BattleSystem { get => battleSystem;}
    public IFieldActionCost ActionCost { get => actionCost;}
    public IFieldHealth Health { get => health;}
}