using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFieldActionCost : IBattleEntryActionCost{
    public void IncreaseMaxCapacity(int amount, FieldEffectDuration duration);
    public void DecreaseMaxCapacity(int amount, FieldEffectDuration duration);
}