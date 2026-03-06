using UnityEngine;

[CreateAssetMenu(fileName = "TransactionChoiceData", menuName = "Scriptable Objects/TransactionChoiceData", order = 0)]
public class TransactionChoiceData : ScriptableObject {
    [SerializeField] private string id;
    [SerializeField, TextArea] private string description;
    [SerializeField, TextArea] private string subDescription;
    

    public string Id { get => id; }
    public string Description { get => description; }
    public string SubDescription { get => subDescription; }
}