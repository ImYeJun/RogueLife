using UnityEngine;

public class BelongingsEntity : MonoBehaviour
{
    [SerializeField] private BelongingsData data;
    [SerializeReference, SubclassSelector] private FieldBelongingsBehaviour fieldBehaviour;
    [SerializeReference, SubclassSelector] private BattleBelongingsBehaviour battleBehaviour;

    public BelongingsData Data { get => data; }
    public FieldBelongingsBehaviour CloneFieldBehaviour() { return fieldBehaviour.Clone(); }
    public BattleBelongingsBehaviour CloneBattleBehaviour() { return battleBehaviour.Clone(); }
}
