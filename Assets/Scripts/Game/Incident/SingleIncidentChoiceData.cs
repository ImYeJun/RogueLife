using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SingleIncidentChoiceData : IIncidentChoiceData
{
    [SerializeField] private string description;
    [SerializeField, TextArea] private string effectDescription;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    public List<DeterminedIncidentChoice> DetermineEffect(FieldContext context)
    {
        var result = new List<DeterminedIncidentChoice>{ new DeterminedIncidentChoice(description, effectDescription, effect) };
        return result;
    }
}