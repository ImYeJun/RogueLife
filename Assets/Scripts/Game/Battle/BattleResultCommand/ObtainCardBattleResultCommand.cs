#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle.BattleResultCommands
{
    public class ObtainCardCommand : BattleResultCommand
    {
        public ObtainCardCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            List<(CardRarity rarity, int weight)> candidateRarity = mainEnemyTier switch
            {
                EnemyTier.NORMAL => new List<(CardRarity rarity, int weight)>() { (CardRarity.COMMON, 100) },
                EnemyTier.ELITE => new List<(CardRarity rarity, int weight)>() { (CardRarity.COMMON, 40), (CardRarity.RARE, 60) },
                EnemyTier.BOSS => new List<(CardRarity rarity, int weight)>() { (CardRarity.RARE, 70), (CardRarity.LEGENDARY, 30) },
                _ => throw new InvalidOperationException($"[ObtainCardCommand] {mainEnemyTier} is not supported.")
            };

            var random = context.Random;

            var determinedRarity = DetermineRewardCardRarity(random, candidateRarity);

            Card? rewardingCard = context.CardDatabase.GetRandomCard(random, determinedRarity, CardType.ANY, CardAttribute.ANY);
            if (rewardingCard is null) { return; }

            var reward = new CardBattleReward(rewardingCard);
            rewardCollector.AddCandidate(reward);
        }

        private CardRarity DetermineRewardCardRarity(Random random, List<(CardRarity rarity, int weight)> candidates)
        {
            if (candidates is null || candidates.Count <= 0) { throw new InvalidOperationException("[BattleNode] Candidates cannot be empty"); }

            int totalWeight = candidates.Sum(candidate => candidate.weight);

            int pivot = random.Next(totalWeight);
            int currentWeight = 0;
            foreach (var candidate in candidates)
            {
                currentWeight += candidate.weight;

                if (currentWeight > pivot) { return candidate.rarity; }
            }

            return candidates[random.Next(candidates.Count)].rarity;
        }
    }
}