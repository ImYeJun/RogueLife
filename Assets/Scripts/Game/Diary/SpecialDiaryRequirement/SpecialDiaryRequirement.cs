using System;
using UnityEngine;

[Serializable]
public abstract class SpecialDiaryRequirement
{
    abstract public bool IsFulfilled(DiaryContext context);
}