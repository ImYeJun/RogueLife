using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BelongingsData", menuName = "Scriptable Objects/BelongingsData")]
public class BelongingsData : ScriptableObject
{
    [SerializeField] string belongingsName;
    [SerializeField] string description;
    [SerializeReference, SubclassSelector] private FieldBelongingsBehaviour fieldBehaviour;
    [SerializeReference, SubclassSelector] private BattleBelongingsBehaviour battleBehaviour;

    public string BelongingsName { get => belongingsName; }
    public string Description { get => description; }

    public FieldBelongingsBehaviour CloneFieldBehaviour() { return fieldBehaviour.Clone(); }
    public BattleBelongingsBehaviour CloneBattleBehaviour() { return battleBehaviour.Clone(); }
}