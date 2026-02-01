using System;
using UnityEngine;

[Serializable]
public class Diary
{
    [SerializeField] private string description;
    [SerializeField] private SpecialDiaryImageType imageType;
    [SerializeField] private string date;

    public Diary(string description, SpecialDiaryImageType imageType, string date)
    {
        this.description = description;
        this.imageType = imageType;
        this.date = date;
    }

    public string Description { get => description; }
    public SpecialDiaryImageType Image { get => imageType; }
    public string Date { get => date; }

    public  bool Equals(Diary operand)
    {
        return 
            description == operand.description
            && imageType == operand.imageType
            && date == operand.date;
    }
}