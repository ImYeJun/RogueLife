using UnityEngine;

[CreateAssetMenu(fileName = "TransactionChoiceData", menuName = "Scriptable Objects/TransactionChoiceData", order = 0)]
public class TransactionChoiceData : ScriptableObject {
    [SerializeField] private string id;
    [SerializeField] private string description;
    [SerializeField] private string subDescription;
    [SerializeReference, SubclassSelector] private IChoiceCondition condition;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    public string Id { get => id; }
    public string Description { get => description; }
    public string SubDescription { get => subDescription; }
    public bool IsInstantEffect => effect.IsInstant;

    public bool IsFulfilled(FieldContext context)
    {
        return condition.IsFulfilled(context);
    }

    public void OnSelected(FieldContext context, Node currentNode)
    {
        if (!IsFulfilled(context)) { return; }

        effect.Execute(context, currentNode);
    }
}