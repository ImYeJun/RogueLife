using UnityEngine;

[CreateAssetMenu(fileName = "DiaryRequirement", menuName = "Scriptable Objects/DiaryRequirement")]
public class DiaryRequirement : ScriptableObject
{
    public bool IsFulfilled(DiaryContext context)
    {
        return false;
    }
}