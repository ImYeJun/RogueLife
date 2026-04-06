using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomCompositeIncidentChoiceData : IIncidentChoiceData
{
    [SerializeField] private List<RandomIncidentChoiceCandidate> candidates;
    [SerializeField] private int pickCount;

    public List<DeterminedIncidentChoice> DetermineEffect(FieldContext context)
    {
        if (candidates.Count < pickCount) { throw new InvalidOperationException("[RandomCompositeIncidentChoiceData] the amount exceed candidates' element count"); }

        var selectedCandidates = new List<DeterminedIncidentChoice>();

        var remainPool = new List<RandomIncidentChoiceCandidate>(candidates);
        for (int i = 0; i < pickCount; i++)
        {
            var selected = SelectCandidate(context, remainPool);
            selectedCandidates.Add(new DeterminedIncidentChoice(selected.Description, selected.EffectDescription, selected.Effect));
            remainPool.Remove(selected);
        }

        return selectedCandidates;
    }

    public RandomIncidentChoiceCandidate SelectCandidate(FieldContext context, List<RandomIncidentChoiceCandidate> pool)
    {
        var random = context.Random;
        
        double totalWeight = 0;
        foreach (var candidate in pool)
        {
            if (candidate.Weight > 0)
            {
                totalWeight += candidate.Weight;
            }
        }

        if (totalWeight <= 0)
        {
            int randomIndex = random.Next(pool.Count);
            return pool[randomIndex];
        }

        double pivot = totalWeight * random.NextDouble();
        double currentWeight = 0;

        foreach (var candidate in pool)
        {
            if (candidate.Weight <= 0) continue;

            currentWeight += candidate.Weight;

            if (currentWeight >= pivot)
            {
                return candidate;
            }
        }

        return pool[pool.Count - 1];
    }
}