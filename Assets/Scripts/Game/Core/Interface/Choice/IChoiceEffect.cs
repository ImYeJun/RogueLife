using System;
using UnityEngine;

public interface IChoiceEffect
{
    public bool IsInstant { get; }
    public abstract void Execute(FieldContext context, Node currentNode);
}