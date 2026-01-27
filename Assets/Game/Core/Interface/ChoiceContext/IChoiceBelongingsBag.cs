using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IChoiceBelongingsBag {
    public bool TryObtainBelongings(Belongings belongings);

}