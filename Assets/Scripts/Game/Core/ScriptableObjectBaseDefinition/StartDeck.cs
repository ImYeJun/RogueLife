using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartDeck", menuName = "Scriptable Objects/StartDeck")]
public class StartDeck : ScriptableObject {
    [System.Serializable]
    public struct CardCountPair {
        public int count;
        public CardEntity entity;
    }

    [SerializeField] private Color32 uniqueColor;
    [SerializeField, TextArea] private string description;
    [SerializeField] private CardAttribute typicalAttribute;
    [SerializeField] private List<CardCountPair> startCards;
    
    public Color UniqueColor { get => uniqueColor; }
    public string Description { get => description; }
    public CardAttribute TypicalAttribute { get => typicalAttribute; }
    public List<CardCountPair> StartCards => startCards;
}