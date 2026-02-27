using System.Collections.Generic;
using Battle.StartEffects;

public class BattleStartEffectSystem : IBattleEventObserveService
{
    private BattleContext context;
    private List<BattleStartEffect> startEffects = new List<BattleStartEffect>();

    public void SetContext(BattleContext context) { this.context = context; }

    public void AddEffect(BattleStartEffect effect)
    {
        if (startEffects.Contains(effect))
        {
            UnityEngine.Debug.LogError("[BattleStartEffectSystem] The given effect is already existing.");
        }

        startEffects.Add(effect);
    }

    public void ApplyEffects(BattleStartEvent payload)
    {
        for (int i = startEffects.Count - 1; i >= 0; i--)
        {
            var effect = startEffects[i];
            
            effect.ApplyEffect(context);

            if (effect.IsExpired)
            {
                startEffects.RemoveAt(i);
            }
        }
    }

    public void SubscribeEventBus(IBattleEventBus eventBus)
    {
        context.EventBus.Subscribe<BattleStartEvent>(ApplyEffects, BattleEventObserverStage.POST);
    }
}