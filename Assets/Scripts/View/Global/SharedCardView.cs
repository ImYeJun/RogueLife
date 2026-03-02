using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SharedCardView : MonoBehaviour
{
    [Serializable]
    public struct CardAsset
    {
        [SerializeField] private CardAttribute attribute;
        [SerializeField] private Sprite frame;
        [SerializeField] private Sprite defaultBackground;

        public CardAttribute Attribute { get => attribute; }
        public Sprite Frame { get => frame; }
        public Sprite DefaultBackground { get => defaultBackground; }
    }

    private Card card;
    [SerializeField] private List<CardAsset> assets;
    [SerializeField] private Image frame;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI cost;

    public Card Card { get => card; }

    public void SetCard(Card card)
    {
        this.card = card;

        var attribute = card.CurrentAttribute;

        var asset = assets.First(asset => asset.Attribute == attribute);

        frame.sprite = asset.Frame;
        background.sprite = asset.DefaultBackground;

        cost.text = card.CurrentActionCost.ToString();
        cardName.text = card.CurrentName;
        description.text = card.CurrentDescription;
    }
}
