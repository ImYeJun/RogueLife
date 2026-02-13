using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncidentDatabase", menuName = "Scriptable Objects/IncidentDatabase")]
public class IncidentDatabase : ScriptableObject, IRunDiaryIncidentDatabaseContext {
    [SerializeField] private List<IncidentData> availableIncidents;

    public List<IncidentData> AvailableIncidents => availableIncidents;
}