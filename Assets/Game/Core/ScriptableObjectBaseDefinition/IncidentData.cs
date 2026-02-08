using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncidentData", menuName = "Scriptable Objects/IncidentData")]
public class IncidentData : ScriptableObject {
    [SerializeField] private string incidentName;
    [SerializeField] private List<IncidentChoiceData> choices;
}