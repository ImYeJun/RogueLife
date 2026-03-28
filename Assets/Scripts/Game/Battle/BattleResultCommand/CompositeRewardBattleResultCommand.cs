#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle.BattleResultCommands
{
    public class CompositeRewardBattleResultCommand : BattleResultCommand
    {
        public CompositeRewardBattleResultCommand(EnemyTier mainEnemyTier) : base(mainEnemyTier)
        {
        }

        public override void Resolve(FieldContext context, Node currentNode, BattleRewardCollector rewardCollector)
        {
            var random = context.Random;

            int cardRewardCount = 0;
            bool hasBelongingsReward = false;
            List<(CardRarity rarity, int weight)> cardRarityWeights = new();
            string cardRewardTitle = "";

            switch (mainEnemyTier)
            {
                case EnemyTier.NORMAL:
                    cardRewardCount = random.Next(100) < 60 ? 1 : 2;
                    cardRarityWeights.Add((CardRarity.COMMON, 100));
                    cardRewardTitle = $"일반 등급의 랜덤 카드 {cardRewardCount}장을 얻는다";
                    break;

                case EnemyTier.ELITE:
                    cardRewardCount = 2;
                    cardRarityWeights.Add((CardRarity.COMMON, 40));
                    cardRarityWeights.Add((CardRarity.RARE, 60));
                    cardRewardTitle = "고급 등급 이하의 랜덤 카드 2장을 얻는다";
                    break;

                case EnemyTier.BOSS:
                    cardRewardCount = 2;
                    cardRarityWeights.Add((CardRarity.RARE, 70));
                    cardRarityWeights.Add((CardRarity.LEGENDARY, 30));
                    hasBelongingsReward = true;
                    cardRewardTitle = "고급 등급 이상의 랜덤 카드 2장을 얻는다";
                    break;

                default:
                    throw new InvalidOperationException($"[CompositeRewardBattleResultCommand] {mainEnemyTier} is not supported.");
            }

            List<IBattleReward> cardRewards = new List<IBattleReward>();
            for (int i = 0; i < cardRewardCount; i++)
            {
                var determinedRarity = DetermineRewardCardRarity(random, cardRarityWeights);
                Card? rewardingCard = context.CardDatabase.GetRandomCard(random, determinedRarity, CardType.ANY, CardAttribute.ANY);

                if (rewardingCard is not null)
                {
                    cardRewards.Add(new CardBattleReward(rewardingCard));
                }
            }
            
            var cardComposite = new CompositeBattleReward(cardRewards, cardRewardTitle);
            rewardCollector.AddCandidate(cardComposite);

            if (hasBelongingsReward)
            {
                var equippingBelongings = context.BelongingsBag.EquippingBelongings;
                Belongings? rewardingBelongings = context.BelongingsDatabase.GetRandomBelongings(random, equippingBelongings);

                if (rewardingBelongings is not null)
                {
                    rewardCollector.AddCandidate(new CompositeBattleReward(
                        new List<IBattleReward>() { new BelongingsBattleReward(rewardingBelongings) },
                        "무작위 보유하지 않은 소지품 1개를 얻는다"
                    ));
                }
            }
        }

        private CardRarity DetermineRewardCardRarity(Random random, List<(CardRarity rarity, int weight)> candidates)
        {
            if (candidates is null || candidates.Count <= 0) 
            { 
                throw new InvalidOperationException("[CompositeRewardBattleResultCommand] Candidates cannot be empty"); 
            }

            int totalWeight = candidates.Sum(candidate => candidate.weight);
            int pivot = random.Next(totalWeight);
            int currentWeight = 0;

            foreach (var candidate in candidates)
            {
                currentWeight += candidate.weight;

                if (currentWeight > pivot) 
                { 
                    return candidate.rarity; 
                }
            }

            return candidates.Last().rarity;
        }
    }
}