using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialDiaryEntity : MonoBehaviour {
    [SerializeField] private SpecialDiaryData data;
    [SerializeReference, SubclassSelector] private List<SpecialDiaryRequirement> requirements = new List<SpecialDiaryRequirement>();

    public string Id { get => data.Id; }
    public string Description { get => data.Description; }
    public SpecialDiaryData Data => data;

    public bool AreRequirementsFulfilled(DiaryContext context)
    {
        return requirements.Count == 0 || requirements.All(requirements => requirements.IsFulfilled(context));
    }
}