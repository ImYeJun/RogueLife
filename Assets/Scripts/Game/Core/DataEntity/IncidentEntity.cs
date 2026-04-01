using System.Collections.Generic;
using UnityEngine;

public class IncidentEntity : MonoBehaviour {
    [SerializeField] private IncidentData data;
    [SerializeReference, SubclassSelector] private List<IIncidentChoiceData> choices;
    
    public string Id { get => data.Id; }
    public Sprite Image { get => data.Image; }
    public string IncidentName { get => data.IncidentName; }
    public IncidentData Data => data;
    
    public List<DeterminedIncidentChoice> DetermineEffect(FieldContext context)
    {
        var result = new List<DeterminedIncidentChoice>();

        foreach (var choice in choices)
        {
            result.AddRange(choice.DetermineEffect(context));
        }
        
        return result;
    }
}