using System;

[Serializable]
public class ChoiceNoneEffect : IChoiceEffect
{
    public void Execute(FieldContext context, Node currentNode)
    {
    }

    public bool IsInstant => true;
}