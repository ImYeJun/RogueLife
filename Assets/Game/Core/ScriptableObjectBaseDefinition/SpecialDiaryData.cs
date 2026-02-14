using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeicalDiaryData", menuName = "Scriptable Objects/SpecialDiaryData")]
public class SpecialDiaryData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string description;
    [SerializeField] private SpecialDiaryImageType imageType; //TODO SpecialDiaryImageType 삭제하기
    [SerializeReference, SubclassSelector] private List<SpecialDiaryRequirement> requirements;
    
    public string Id { get => id; }
    public string Description { get => description; }
    public SpecialDiaryImageType Image { get => imageType; }

    public SpecialDiaryData(string description, SpecialDiaryImageType imageType)
    {
        this.description = description;
        this.imageType = imageType;
    }

    public bool AreRequirementsFulfilled(DiaryContext context)
    {
        return requirements.All(requirements => requirements.IsFulfilled(context));
    }
}