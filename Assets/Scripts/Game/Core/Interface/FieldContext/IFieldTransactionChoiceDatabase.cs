public interface IFieldTransactionChoiceDatabase
{
    public bool TryGetRandomData(FieldContext context, TransactionChoiceOrder order, out TransactionChoiceEntity choiceData);
}