using System.Collections.Generic;
using UnityEngine;

public class IncidentEntity : MonoBehaviour {
    [SerializeField] private IncidentData data;
    [SerializeReference, SubclassSelector] private List<IIncidentChoiceData> choices;
    
    public string Id { get => data.Id; }
    public string IncidentName { get => data.IncidentName; }
    public IncidentData Data => data;
    
    public List<DeterminedIncidentChoiceData> DetermineEffect(FieldContext context)
    {
        var result = new List<DeterminedIncidentChoiceData>();

        foreach (var choice in choices)
        {
            result.AddRange(choice.DetermineEffect(context));
        }
        
        return result;
    }
}