using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomCompositeIncidentChoiceData : IIncidentChoiceData
{
    [SerializeField] private List<RandomIncidentChoiceCandidate> candidates;
    private IIncidentChoiceData selectedCandidate;

    public void OnSelected(FieldContext context)
    {
        if (candidates == null || candidates.Count == 0) { return; }
        
        var selectedCandidate = SelectCandidate(context);

        selectedCandidate?.Effect.Execute(context);
    }

    public RandomIncidentChoiceCandidate SelectCandidate(FieldContext context)
    {
        var random = context.Random;
        
        double totalWeight = 0;
        foreach (var candidate in candidates)
        {
            if (candidate.Weight > 0)
            {
                totalWeight += candidate.Weight;
            }
        }

        if (totalWeight <= 0)
        {
            int randomIndex = random.Next(candidates.Count);
            return candidates[randomIndex];
        }

        double pivot = totalWeight * random.NextDouble();
        double currentWeight = 0;

        foreach (var candidate in candidates)
        {
            if (candidate.Weight <= 0) continue;

            currentWeight += candidate.Weight;

            if (currentWeight >= pivot)
            {
                return candidate;
            }
        }

        return candidates[candidates.Count - 1];
    }
}