using UnityEngine;

public class TransactionChoiceEntity : MonoBehaviour {
    [SerializeField] private TransactionChoiceData data;
    [SerializeReference, SubclassSelector] private IChoiceCondition condition;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    
    public TransactionChoiceData Data { get => data;}
    public string Id { get => data.Id; }
    public string Description { get => data.Description; }
    public string SubDescription { get => data.SubDescription; }

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