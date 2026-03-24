
using System.Linq;

namespace Battle.Enemies.Actions.Shared
{
    public class DumpPlayerHandCard : EnemyAction
    {
        private int dumpCardCount;

        public DumpPlayerHandCard(string id, IEnemyBehaviourOwner owner, int dumpCardCount = 1) : base(id, owner, BattleEnemyActionType.Effect)
        {
            this.dumpCardCount = dumpCardCount;
        }

        public override void Execute(BattleContext context)
        {
            var handCards = context.HandDeck.GetCards();
            if (handCards.Count <= 0) { return; }

            var suffled = handCards.OrderBy(card => context.Random.Next());
            var selectedCards = suffled.Take(dumpCardCount);

            foreach (var card in selectedCards)
            {
                var dumpCardAction = new MoveCardToDeckBattleAction(card, BattleDeckType.GRAVE);
                context.ActionScheduler.Enqueue(dumpCardAction);
            }
        }
    }
}