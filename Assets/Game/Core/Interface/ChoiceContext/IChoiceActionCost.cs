
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IChoiceActionCost {
    public void IncreaseMaxActionCost(int amount);
    public void DecreaseMaxActionCost(int amount);

}