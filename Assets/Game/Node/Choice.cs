using System.Collections.Generic;
using UnityEngine;

public class Choice
{
    private string description;
    private string subDescription;
    private List<ChoiceEffect> effects;

    public string Description { get => description; }
    public string SubDescription { get => subDescription; }
    public List<ChoiceEffect> Effects { get => effects; }
}