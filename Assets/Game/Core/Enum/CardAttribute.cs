using System;

[Flags]
public enum CardAttribute
{
    PHYSICAL = 1 << 0, 
    MAGIC = 1 << 1, 
    LUCK = 1 << 2,
    ANY = PHYSICAL | MAGIC | LUCK
}