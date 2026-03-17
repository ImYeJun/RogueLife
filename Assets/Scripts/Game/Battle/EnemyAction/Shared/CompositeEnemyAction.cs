using System.Collections.Generic;

namespace Battle.Enemies.Actions.Shared
{
    public class CompositeEnemyAction : EnemyAction
    {
        private List<EnemyAction> actions;

        public CompositeEnemyAction(string id, IEnemyBehaviourOwner owner, List<EnemyAction> actions) : base(id, owner)
        {
            this.actions = actions;
        }

        public override void Execute(BattleContext context)
        {
            foreach (var action in actions)
            {
                action.Execute(context);
            }
        }
    }
}