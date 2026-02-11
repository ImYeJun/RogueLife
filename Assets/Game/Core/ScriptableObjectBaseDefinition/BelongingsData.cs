using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BelongingsData", menuName = "Scriptable Objects/BelongingsData")]
public class BelongingsData : ScriptableObject
{
    [SerializeField] List<IBattleAction> actions;
    [SerializeField] string belongingsName;
    [SerializeField] string description;

    public string BelongingsName { get => belongingsName; }
    public string Description { get => description; }

    public void Execute(BattleContext context)
    {
        foreach (IBattleAction action in actions)
        {
            action.Execute(context);
        }
    }
}