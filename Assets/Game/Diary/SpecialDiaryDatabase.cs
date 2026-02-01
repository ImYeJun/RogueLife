using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialDiaryDatabase", menuName = "Scriptable Objects/SpecialDiaryDatabase")]
public class SpecialDiaryDatabase : ScriptableObject
{
    [SerializeField] private List<SpeicalDiaryData> speicalDiaryDatas;

    public bool TryGetSpecialDiaryData(DiaryContext context, out SpeicalDiaryData speicalDiaryData)
    {
        foreach (SpeicalDiaryData element in speicalDiaryDatas)
        {
            if (element.AreRequirementsFullfilled(context))
            {
                speicalDiaryData = element;
                return true;
            }
        }

        speicalDiaryData = null;
        return false;
    }
}