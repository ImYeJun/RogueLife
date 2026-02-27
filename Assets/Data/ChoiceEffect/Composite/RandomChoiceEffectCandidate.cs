using System;
using UnityEngine;

[Serializable]
public class RandomChoiceEffectCandidate
{
    [SerializeReference, SubclassSelector] IChoiceEffect effect;
    [SerializeField, Min(0)] int weight;

    public RandomChoiceEffectCandidate() {}

    public IChoiceEffect Effect { get => effect; }
    public int Weight { get => weight; }
}