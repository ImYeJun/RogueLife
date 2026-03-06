using System.Collections.Generic;
using UnityEngine;

public class IncidentData : ScriptableObject {
    [SerializeField] private string id;
    [SerializeField] private string incidentName;
    
    public string Id { get => id; }
    public string IncidentName { get => incidentName; }
}