using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Battle.Cards.Casters;

namespace Battle.Cards.Behaviours
{
    [Serializable]
    public class DestinyDiceRoll : CardBattleBehaviour<SingleEnemyCardTarget, SingleEnemyCardTarget>
    {
        [Obsolete("This constructor is for Unity Serialization only. Use Clone() instead.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DestinyDiceRoll() {}
        private DestinyDiceRoll(ICardBehaviourOwner owner, CardTargetType targetType, CardTargetType reflectionTargetType) 
        : base(owner, targetType, reflectionTargetType) { }

        public override CardBattleBehaviour Clone(ICardBehaviourOwner owner)
        {
            return new DestinyDiceRoll(owner, targetType, reflectionTargetType);
        }

        public override bool OnIsAbleToUse(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override bool OnIsAbleToUseReflect(BattleContext context, SingleEnemyCardTarget target)
        {
            return true;
        }

        public override void OnDraw(BattleContext context)
        {
        }

        private struct CandidateNumber
        {
            public int number;
            public int weight;

            public CandidateNumber(int number, int weight)
            {
                this.number = number;
                this.weight = weight;
            }
        }
        protected override void OnExecute(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            List<CandidateNumber> candidates = new List<CandidateNumber>
            {
                new CandidateNumber(1, 30),
                new CandidateNumber(2, 25),
                new CandidateNumber(3, 20),
                new CandidateNumber(4, 13),
                new CandidateNumber(5, 8),
                new CandidateNumber(6, 3)
            };

            ExecuteCommonAction(context, caster, target, candidates);
        }

        protected override void OnExecuteReflection(BattleContext context, CardCaster caster, SingleEnemyCardTarget target)
        {
            List<CandidateNumber> candidates = new List<CandidateNumber>
            {
                new CandidateNumber(3, 40),
                new CandidateNumber(4, 30),
                new CandidateNumber(5, 20),
                new CandidateNumber(6, 10)
            };
            
            ExecuteCommonAction(context, caster, target, candidates);
        }

        private void ExecuteCommonAction(BattleContext context, CardCaster caster, SingleEnemyCardTarget target, List<CandidateNumber> candidates)
        {
            var selecetdCandidate = SelecetCandidate(context.Random, candidates);
            
            for (int i = 0; i < selecetdCandidate.number; i++)
            {
                int damage = 15;
                var hurtAction = new RequestHurtEntityBattleAction(owner.GetAsHurtSource(caster), damage, target.Enemy);
                context.ActionScheduler.Enqueue(hurtAction);
            }
        }

        private CandidateNumber SelecetCandidate(Random random, List<CandidateNumber> candidates)
        {
            int totalWeight = candidates.Sum(candidate => candidate.weight);
            int pivot = random.Next(totalWeight);

            int currentWeight = 0;
            foreach (var candidate in candidates)
            {
                currentWeight += candidate.weight;

                if (currentWeight > pivot) { return candidate; }
            }

            return candidates.Last();
        }
    }
}