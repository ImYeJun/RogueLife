using System;
using UnityEngine;

[Serializable]
public class IncidentChoiceData
{
    [SerializeField] private string description;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    public void OnSelected(FieldContext context)
    {
        effect.Execute(context);
    }
}