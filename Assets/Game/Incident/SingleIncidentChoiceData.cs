using System;
using UnityEngine;

[Serializable]
public class SingleIncidentChoiceData : IIncidentChoiceData
{
    [SerializeField] private string description;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    public string Description { get => description; }

    public void OnSelected(FieldContext context)
    {
        effect.Execute(context);
    }
}