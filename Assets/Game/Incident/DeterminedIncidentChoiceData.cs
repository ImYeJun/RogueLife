public class DeterminedIncidentChoiceData
{
    private string description;
    private IChoiceEffect effect;

    public DeterminedIncidentChoiceData(string description, IChoiceEffect effect)
    {
        this.description = description;
        this.effect = effect;
    }

    public void OnSelected(FieldContext context)
    {
        effect.Execute(context);
    }

    public string Description { get => description; }
}