
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IChoiceDeck {
    public bool TryObtainCard(Card card);
    public bool TryRemoveCard(Card card, DeckType deckType);
    public bool TryRemoveCard(Card card);
    public void IncreaseMaxCardTypeCount(int amount);
    public void DecreaseMaxCardTypeCount(int amount);

}