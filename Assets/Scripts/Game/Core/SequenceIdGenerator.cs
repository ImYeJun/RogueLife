public class SequenceIdGenerator {
    private int currentSequenceId = 0;

    public void Clear()
    {
        currentSequenceId = 0;
    }

    public int GetNextId()
    {
        return currentSequenceId++;
    }
}