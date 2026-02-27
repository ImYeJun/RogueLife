using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SingleIncidentChoiceData : IIncidentChoiceData
{
    [SerializeField] private string description;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    public List<DeterminedIncidentChoiceData> DetermineEffect(FieldContext context)
    {
        var result = new List<DeterminedIncidentChoiceData>{ new DeterminedIncidentChoiceData(description, effect) };
        return result;
    }
}