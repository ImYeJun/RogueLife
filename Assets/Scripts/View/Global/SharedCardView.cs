using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SharedCardView : MonoBehaviour
{
    [Serializable]
    public struct CardAttributeAsset
    {
        [SerializeField] private CardAttribute attribute;
        [SerializeField] private Sprite reflectionIcon;
        [SerializeField] private Sprite frame;
        [SerializeField] private Sprite defaultBackground;

        public CardAttribute Attribute { get => attribute; }
        public Sprite Frame { get => frame; }
        public Sprite DefaultBackground { get => defaultBackground; }
        public Sprite ReflectionIcon { get => reflectionIcon; }
    }
    
    [Serializable]
    public struct CardTypeAsset
    {
        [SerializeField] private CardType type;
        [SerializeField] private Sprite image;

        public CardType Type { get => type; }
        public Sprite Image { get => image; }
    }

    private Card card;

    [Header("Assets")]
    [SerializeField] private List<CardAttributeAsset> attributeAssets;
    [SerializeField] private List<CardTypeAsset> typeAssets;

    [Header("Reference")]
    [SerializeField] private Image frame;
    [SerializeField] private Image background;
    [SerializeField] private Image reflectionIconImage;
    [SerializeField] private Image typeImage;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI cost;

    [Header("Action Cost View")]
    [SerializeField] private Color32 normalCostTextColor;
    [SerializeField] private Color32 increasedCostTextColor;
    [SerializeField] private Color32 decreasedCostTextColor;
    
    public Card Card { get => card; }

    public void SetCard(Card card)
    {
        if (card == null)
        {
            throw new ArgumentNullException(nameof(card), "[SharedCardView/] The provided card is null.");
        }

        if (this.card != null)
        {
            this.card.OnCostChanged -= OnActionCostChanged;
            this.card.OnReflectionChanged -= OnReflectionChanged;
        }

        this.card = card;
        this.card.OnCostChanged += OnActionCostChanged;
        this.card.OnReflectionChanged += OnReflectionChanged;
        
        DrawSync();
    }

    private void OnDestroy()
    {
        if (card != null)
        {
            card.OnCostChanged -= OnActionCostChanged;
            card.OnReflectionChanged -= OnReflectionChanged;
        }
    }

    //TODO This code may confused with OnCardCost BattleViewEvent. It's because BattleCard is not Separated from the Card class! Refactor it!
    public void OnActionCostChanged()
    {
        DrawCost(card.CurrentActionCost);
    }
    
    public void OnReflectionChanged()
    {
        DrawDescription(card.IsReflectionApplied);
    }

    private void DrawSync()
    {
        DrawByAttribute();
        DrawByType();

        DrawCost(card.CurrentActionCost);
        
        cardName.text = card.CurrentName;
        DrawDescription(card.IsReflectionApplied);
    }

    private void DrawByAttribute()
    {
        var attribute = card.CurrentAttribute;

        var asset = attributeAssets.First(asset => asset.Attribute == attribute);
        frame.sprite = asset.Frame;
        background.sprite = card.Data.Background ?? asset.DefaultBackground;
    }

    private void DrawByType()
    {
        var type = card.CurrentType;

        var asset = typeAssets.First(asset => asset.Type == type);
        typeImage.sprite = asset.Image;
    }

    public void DrawCost(int currentCost)
    {
        cost.text = currentCost.ToString();

        cost.color = currentCost.CompareTo(card.BaseActionCost) switch
        {
            1 => increasedCostTextColor,  
            -1 => decreasedCostTextColor,  
            _ => normalCostTextColor      
        };
    }

    //TODO Erase these code when Card is splited into BattleCard and SheduleCard (Or something...)
    public void LinkCosySync()
    {
        card.OnCostChanged += OnActionCostChanged;
        card.OnReflectionChanged += OnReflectionChanged;
    }
    public void UnlinkSync()
    {
        card.OnCostChanged -= OnActionCostChanged;
        card.OnReflectionChanged -= OnReflectionChanged;
    }

    public void DrawDescription(bool isReflection)
    {
        description.text = isReflection ? card.ReflectionEffectDescription : card.NormalEffectDescription;
        
        reflectionIconImage.gameObject.SetActive(isReflection);
        if (isReflection)
        {
            var attribute = card.CurrentAttribute;
            var asset = attributeAssets.First(asset => asset.Attribute == attribute);

            reflectionIconImage.sprite = asset.ReflectionIcon;
        }
    }
}