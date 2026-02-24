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
    protected List<Pattern> availablePatterns = new List<Pattern>();

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

        var remainActions = FillRemainAction(context.Random, remainActionCount, result);
        result.AddRange(remainActions);

        return ValidateActions(result);
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
    private List<EnemyAction> FillRemainAction(Random random, int remainActionCount, List<EnemyAction> alreadyPlannedActions)
    {
        var result = new List<EnemyAction>();
        if (remainActionCount <= 0) { return result; }

        var actions = availableActions.Values.ToList();
        if (actions.Count <= 0)
        {
            UnityEngine.Debug.LogWarning("[BattleEnemyBehaviour] There's no available actions.");
            return result;
        }

        int actionCount = 0;
        int tryCount = 0;
        
        var actionsOncePerTurn = new HashSet<EnemyAction>(
            alreadyPlannedActions.Where(action => action.IsOncePerTurn)
        );

        while (actionCount < remainActionCount)
        {
            var chosenAction = actions[random.Next(actions.Count)];

            if (actionsOncePerTurn.Contains(chosenAction)) {
                if (++tryCount >= Constant.MAX_ACTION_CHOOSE_TRY_COUNT)
                {
                    UnityEngine.Debug.LogWarning("[BattleEnemyBehaviour] Exceed max action choose try count");
                    break;
                }
                continue;
            }
            tryCount = 0;
            
            result.Add(chosenAction);

            if (chosenAction.IsOncePerTurn)
            {
                actionsOncePerTurn.Add(chosenAction);
            }

            actionCount++;
        }

        return result;
    }
    private List<EnemyAction> ValidateActions(List<EnemyAction> result)
    {
        var finalResult = new List<EnemyAction>();

        foreach (var action in result)
        {
            finalResult.Add(action);

            if (action.IsLastAction) break;
        }

        return finalResult;
    }
    
    public abstract BattleEnemyBehaviour Clone(IEnemyBehaviourOwner newOwner);
    public abstract void OnOwnerSpawned(BattleContext context);
    public abstract void OnOwnerDied(BattleContext context);
}
