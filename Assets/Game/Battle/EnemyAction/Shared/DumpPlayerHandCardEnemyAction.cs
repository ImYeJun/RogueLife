
namespace Battle.Enemies.Actions.Shared
{
    public class DumpPlayerHandCard : EnemyAction
    {
        public DumpPlayerHandCard(IEnemyBehaviourOwner owner) : base(owner)
        {
        }

        public override void Execute(BattleContext context)
        {
            var handCards = context.HandDeck.GetCards();
            if (handCards.Count <= 0) { return; }

            var selectedCard = handCards[context.Random.Next(handCards.Count)];
            var dumpCardAction = new MoveCardToDeckBattleAction(selectedCard, BattleDeckType.GRAVE);

            context.ActionScheduler.Enqueue(dumpCardAction);
        }
    }
}