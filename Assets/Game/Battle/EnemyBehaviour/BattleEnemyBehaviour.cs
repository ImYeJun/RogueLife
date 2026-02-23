using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enemies.Actions;

[Serializable]
public abstract class BattleEnemyBehaviour
{
    protected IEnemyBehaviourOwner owner;
    protected Dictionary<string, EnemyAction> availableActions = new Dictionary<string, EnemyAction>();
    //* <actionId, EnemyAcion>

    protected BattleEnemyBehaviour() {}
    protected BattleEnemyBehaviour(IEnemyBehaviourOwner owner)
    {
        this.owner = owner;
    }
    
    protected struct Pattern
    {
        public Pattern(List<string> preset, Func<BattleContext, int, bool> condition)
        {
            Preset = preset;
            Condition = condition;
        }

        public List<string> Preset { get; private set; }
        //* List<actionId>
        public Func<BattleContext, int , bool> Condition { get; private set; }
        //* param : context, remainActionCount
        //* return : isAvailable
    }
    protected List<Pattern> availablePatterns;

    public List<EnemyAction> PlanAction(BattleContext context)
    {
        var result = new List<EnemyAction>();

        int remainActionCount = CalculateActionCount(context.Random);

        var pattern = GetAvailablePattern(context, remainActionCount);

        if (pattern != null)
        {
            var patternPreset = pattern.Value.Preset;

            foreach (var actionId in patternPreset)
            {
                if (remainActionCount <= 0) { break; }

                if (availableActions.TryGetValue(actionId, out EnemyAction action))
                {
                    result.Add(action);
                    remainActionCount--;
                }
                else
                {
                    throw new InvalidOperationException($"[BattleEnemyBehaviour] There is no available action. Id : {actionId}");
                }
            }
        }

        var remainActions = FillRemainAction(context.Random, remainActionCount);
        result.AddRange(remainActions);

        return result;
    }
    protected abstract int CalculateActionCount(Random random);
    protected Pattern? GetAvailablePattern(BattleContext context, int remainActionCount)
    {
        var validPatterns = availablePatterns
            .Where(p => p.Condition.Invoke(context, remainActionCount))
            .OrderByDescending(p => p.Preset.Count)
            .ToList();

        if (validPatterns.Count > 0)
        {
            return validPatterns.First();
        }

        return null;
    }
    private List<EnemyAction> FillRemainAction(Random random, int remainActionCount)
    {
        var result = new List<EnemyAction>();
        if (remainActionCount <= 0) { return result; }

        var actions = availableActions.Values.ToList();
        if (actions.Count <= 0)
        {
            UnityEngine.Debug.LogWarning("[BattleEnemyBehaviour] There's no available actions.");
            return result;
        }

        for (int i = 0; i < remainActionCount; i++)
        {
            var chosenAction = actions[random.Next(actions.Count)];
            result.Add(chosenAction);
        }

        return result;
    } 

    public abstract BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner);
}
