using System;

[Serializable]
public class NoneChoideCondition : IChoiceCondition
{
    public bool IsFulfilled(FieldContext context)
    {
        return true;
    }
}