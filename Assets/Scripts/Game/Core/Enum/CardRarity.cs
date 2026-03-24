public enum CardRarity
{
    ANY, COMMON, RARE, LEGENDARY
}

public static class CardRarityExtenstions
{
    public static string ToKorean(CardRarity rarity)
    {
        return rarity switch
        {
            CardRarity.ANY     => "전체",
            CardRarity.COMMON  => "일반",
            CardRarity.RARE  => "희귀",
            CardRarity.LEGENDARY  => "전설",
            _                => rarity.ToString()
        };
    }
}