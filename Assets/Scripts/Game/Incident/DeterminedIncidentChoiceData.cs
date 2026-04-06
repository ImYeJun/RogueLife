public class DeterminedIncidentChoice
{
    private string description;
    private string effectDescription;
    private IChoiceEffect effect;

    public DeterminedIncidentChoice(string description, string effectDescription, IChoiceEffect effect)
    {
        this.description = description;
        this.effectDescription = effectDescription;
        this.effect = effect;
    }

    public void OnSelected(FieldContext context, Node currentNode)
    {
        effect.Execute(context, currentNode);
    }

    public string Description { get => description; }
    public string EffectDescription { get => effectDescription; }
    public bool IsInstantEffect => effect.IsInstant;
}