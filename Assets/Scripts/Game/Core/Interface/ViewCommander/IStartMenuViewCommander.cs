public interface IStartMenuViewCommander : IViewCommander
{
    void FixStartDeck(StartDeck startDeck);
    public void RequestStartDeckSelect();
}