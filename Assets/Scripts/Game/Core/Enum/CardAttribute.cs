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
}