using UnityEngine;

public class CardEntity : MonoBehaviour {
    [SerializeField] private CardData data;
    [SerializeReference, SubclassSelector] private CardBattleBehaviour battleBehaviour;

    public string Id { get => data.Id; }
    public CardData Data { get => data; }
    public CardBattleBehaviour CloneBattleBehaviour(ICardBehaviourOwner owner) { return battleBehaviour.Clone(owner); }
}