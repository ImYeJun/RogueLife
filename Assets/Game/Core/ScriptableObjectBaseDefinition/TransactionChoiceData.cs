using UnityEngine;

[CreateAssetMenu(fileName = "TransactionChoiceData", menuName = "Scriptable Objects/TransactionChoiceData", order = 0)]
public class TransactionChoiceData : ScriptableObject {
    [SerializeField] private string description;
    [SerializeField] private string subDescription;
    [SerializeReference, SubclassSelector] private IChoiceEffect effect;

    public void OnSelcted(FieldContext context)
    {
        effect.Execute(context);
    }
}