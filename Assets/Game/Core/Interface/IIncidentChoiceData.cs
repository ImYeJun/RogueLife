using System;
using System.Collections.Generic;
using UnityEngine;

public interface IIncidentChoiceData
{
    public List<DeterminedIncidentChoiceData> DetermineEffect(FieldContext context);
}