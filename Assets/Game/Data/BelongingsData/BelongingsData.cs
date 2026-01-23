using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BelongingsData", menuName = "Scriptable Objects/BelongingsData")]
public class BelongingsData : ScriptableObject
{
    [SerializeField] List<BattleAction> actions;
    [SerializeField] string belongingsName;
    [SerializeField] string description;

    public string BelongingsName { get => belongingsName; }
    public string Description { get => description; }
}