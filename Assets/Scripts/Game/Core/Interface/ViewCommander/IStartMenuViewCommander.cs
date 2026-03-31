using System.Collections.Generic;

public interface IStartMenuViewCommander : IViewCommander, IStartMenuDiaryCommander
{
    void FixStartDeck(StartDeck startDeck);
    public void RequestStartDeckSelect();
}
    