using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeicalDiaryData", menuName = "Scriptable Objects/SpeicalDiaryData")]
public class SpeicalDiaryData : ScriptableObject
{
    [SerializeField] private string description;
    [SerializeField] private Sprite image;
    [SerializeField] private List<DiaryRequirement> requirements;
    
    public string Description { get => description; }
    public Sprite Image { get => image; }

    public SpeicalDiaryData(string description, Sprite image)
    {
        this.description = description;
        this.image = image;
    }

    public bool AreRequirementsFullfilled(DiaryContext context)
    {
        return requirements.All(requirements => requirements.isFullfilled(context));
    }
}