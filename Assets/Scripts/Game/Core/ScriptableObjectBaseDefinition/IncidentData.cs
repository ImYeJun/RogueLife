using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncidentData : ScriptableObject {
    [SerializeField] private string id;
    [SerializeField] private string incidentName;
    [SerializeField] private Sprite image; 
    
    public string Id { get => id; }
    public string IncidentName { get => incidentName; }
    public Sprite Image { get => image; }
}