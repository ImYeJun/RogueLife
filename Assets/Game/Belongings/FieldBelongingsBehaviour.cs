using System;

[Serializable]
public abstract class FieldBelongingsBehaviour
{
    public abstract void OnEquipped(FieldContext context);
    public abstract void OnUnqeuipped(FieldContext context);
    public abstract FieldBelongingsBehaviour Clone();
}