using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BelongingsData", menuName = "Scriptable Objects/BelongingsData")]
public class BelongingsData : ScriptableObject
{    
    [SerializeField] private string id;
    [SerializeField] string belongingsName;
    [SerializeField] Sprite image;
    [SerializeField, TextArea] string description;
    [SerializeReference, SubclassSelector] private FieldBelongingsBehaviour fieldBehaviour;
    [SerializeReference, SubclassSelector] private BattleBelongingsBehaviour battleBehaviour;

    public string Id { get => id; }
    public string BelongingsName { get => belongingsName; }
    public Sprite Image { get => image; }
    public string Description { get => description; }

    public FieldBelongingsBehaviour CloneFieldBehaviour() { return fieldBehaviour.Clone(); }
    public BattleBelongingsBehaviour CloneBattleBehaviour() { return battleBehaviour.Clone(); }
}