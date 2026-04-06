using System;
using UnityEngine;

[Serializable]
public class RandomIncidentChoiceCandidate
{
    [SerializeReference, SubclassSelector] IChoiceEffect effect;
    [SerializeField] private string description;
    [SerializeField, TextArea] private string effectDescription;
    [SerializeField, Min(0)] int weight;

    public IChoiceEffect Effect { get => effect; }
    public int Weight { get => weight; }
    public string Description { get => description; }
    public string EffectDescription { get => effectDescription; }
}