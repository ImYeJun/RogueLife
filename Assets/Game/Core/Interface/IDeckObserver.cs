using System.Collections.Generic;

public interface IDeckObserver
{
    public void OnEquipped(List<Card> owningCards);
    public void OnCardEquipped(Card card);
    public void OnUnequipped(List<Card> owningCards);
}