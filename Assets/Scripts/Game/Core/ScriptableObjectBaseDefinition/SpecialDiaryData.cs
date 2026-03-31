using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeicalDiaryData", menuName = "Scriptable Objects/SpecialDiaryData")]
public class SpecialDiaryData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private Sprite image;
    [SerializeField] private string specialDiaryName;
    [SerializeField] private string description;
    [SerializeField] private List<string> requirements;
    
    public string Id { get => id; }
    public Sprite Image { get => image; }
    public string Name { get => specialDiaryName; }
    public string Description { get => description; }
    public List<string> Requirements { get => requirements; }
}