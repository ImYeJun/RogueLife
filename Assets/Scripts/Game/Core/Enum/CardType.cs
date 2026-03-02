using System;

public enum CardType
{
    ANY, ATTACK, EFFECT, DEFENSE, SPECIAL, TIME
}

public static class CardTypeExtensions{
    public static string ToKorean(CardType type)
    {
        return type switch
        {
            CardType.ANY     => "전체",
            CardType.ATTACK  => "공격",
            CardType.EFFECT  => "효과",
            CardType.DEFENSE => "방어",
            CardType.SPECIAL => "특수",
            CardType.TIME    => "시간",
            _                => type.ToString()
        };
    }
}