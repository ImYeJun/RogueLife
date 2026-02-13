using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialDiaryDatabase", menuName = "Scriptable Objects/SpecialDiaryDatabase")]
public class SpecialDiaryDatabase : ScriptableObject
{
    [SerializeField] private List<SpecialDiaryData> speicalDiaryDatas;

    public bool TryGetSpecialDiaryData(DiaryContext context, out SpecialDiaryData speicalDiaryData)
    {
        foreach (SpecialDiaryData element in speicalDiaryDatas)
        {
            if (element.AreRequirementsFulfilled(context))
            {
                speicalDiaryData = element;
                return true;
            }
        }

        speicalDiaryData = null;
        return false;
    }
}