using System;

public enum CardAttribute
{
    ANY, PHYSICAL, MAGIC, LUCK
}

public static class CardAttributeExtensions
{
    public static string ToKorean(this CardAttribute attribute)
    {
        return attribute switch
        {
            CardAttribute.ANY      => "전체",
            CardAttribute.PHYSICAL => "물리",
            CardAttribute.MAGIC    => "마법",
            CardAttribute.LUCK     => "운",
            _                      => attribute.ToString()
        };
    }

    public static int GetTextIconIndex(CardAttribute attribute)
    {
        return attribute switch
        {
            CardAttribute.PHYSICAL => 0,
            CardAttribute.MAGIC    => 1,
            CardAttribute.LUCK     => 2,
            _                      => -1
        };
    }
}