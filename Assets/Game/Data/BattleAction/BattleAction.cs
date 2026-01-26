using UnityEngine;

[CreateAssetMenu(fileName = "BattleAction", menuName = "Scriptable Objects/BattleAction")]
public abstract class BattleAction : ScriptableObject
{
    public abstract void Execute(BattleContext context);
}