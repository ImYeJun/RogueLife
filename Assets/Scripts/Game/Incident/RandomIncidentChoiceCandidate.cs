using System;
using UnityEngine;

[Serializable]
public class RandomIncidentChoiceCandidate
{
    [SerializeReference, SubclassSelector] IChoiceEffect effect;
    [SerializeField] private string description;
    [SerializeField, Min(0)] int weight;

    public IChoiceEffect Effect { get => effect; }
    public int Weight { get => weight; }
    public string Description { get => description; }
}