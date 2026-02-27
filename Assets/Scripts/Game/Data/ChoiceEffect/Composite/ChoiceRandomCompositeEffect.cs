using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceRandomCompositeEffect : IChoiceEffect
{
    [SerializeField] private List<RandomChoiceEffectCandidate> candidates;
    [SerializeField, Min(0)] private int pickCount;

    private bool isInstantOutcome = true;

    public ChoiceRandomCompositeEffect() {}

    public bool IsInstant => isInstantOutcome;

    public void Execute(FieldContext context, Node currentNode)
    {
        if (candidates == null || candidates.Count <= 0) { throw new InvalidOperationException($"Candidates cannot be empty"); }
        if (candidates.Count < pickCount) { throw new InvalidOperationException($"Pick count({pickCount}) cannot exceed candidates count({candidates.Count})"); }

        isInstantOutcome = true;

        var pool = new List<RandomChoiceEffectCandidate>(candidates);
        for (int i = 0; i < pickCount; i++)
        {
            var selectedCandidate = SelectCandidate(pool, context.Random);

            if (selectedCandidate != null)
            {
                if (!selectedCandidate.Effect.IsInstant)
                {
                    isInstantOutcome = false;
                }

                selectedCandidate.Effect.Execute(context, currentNode);
            }
        }
    }

    //* Weighted Random Algorithm
    private RandomChoiceEffectCandidate SelectCandidate(List<RandomChoiceEffectCandidate> currentPool, System.Random random)
    {
        if (currentPool.Count == 0) return null;
        
        int totalWeight = 0;
        foreach (var candidate in currentPool)
        {
            totalWeight += candidate.Weight;
        }

        if (totalWeight <= 0)
        {
            int randomIndex = random.Next(currentPool.Count);
            var fallbackItem = currentPool[randomIndex];
            currentPool.RemoveAt(randomIndex);
            return fallbackItem;
        }

        double pivot = random.NextDouble() * totalWeight;
        double currentWeight = 0;

        for (int i = 0; i < currentPool.Count; i++)
        {
            currentWeight += currentPool[i].Weight;
            
            if (currentWeight >= pivot)
            {
                var selected = currentPool[i];
                currentPool.RemoveAt(i);
                return selected;
            }
        }

        var lastItem = currentPool[currentPool.Count - 1];
        currentPool.RemoveAt(currentPool.Count - 1);
        return lastItem;
    }
}