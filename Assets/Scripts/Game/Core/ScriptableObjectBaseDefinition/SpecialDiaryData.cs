using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeicalDiaryData", menuName = "Scriptable Objects/SpecialDiaryData")]
public class SpecialDiaryData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string description;
    
    public string Id { get => id; }
    public string Description { get => description; }
}