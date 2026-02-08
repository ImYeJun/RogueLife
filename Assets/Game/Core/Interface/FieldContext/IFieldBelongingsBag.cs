using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IFieldBelongingsBag {
    public bool TryObtainBelongings(Belongings belongings);
}