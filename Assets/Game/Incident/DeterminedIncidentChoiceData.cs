public class DeterminedIncidentChoiceData
{
    private string description;
    private IChoiceEffect effect;

    public DeterminedIncidentChoiceData(string description, IChoiceEffect effect)
    {
        this.description = description;
        this.effect = effect;
    }

    public void OnSelected(FieldContext context, Node currentNode)
    {
        effect.Execute(context, currentNode);
    }

    public string Description { get => description; }
    public bool IsInstantEffect => effect.IsInstant;
}