using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncidentData", menuName = "Scriptable Objects/IncidentData")]
public class IncidentData : ScriptableObject {
    [SerializeField] private string id;
    [SerializeField] private string incidentName;
    [SerializeReference, SubclassSelector] private List<IIncidentChoiceData> choices;
    
    public string Id { get => id; }
    public string IncidentName { get => incidentName; }
    public List<IIncidentChoiceData> Choices { get => choices; }
}