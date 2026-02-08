using System;

[Flags]
public enum CardType
{
    ATTACK = 1 << 0, 
    DEFENSE = 1 << 1, 
    BUFF = 1 << 2, 
    DEBUFF = 1 << 3, 
    SPECIAL = 1 << 4,
    TIME = 1 << 5,
    ANY = ATTACK | DEFENSE | BUFF | DEBUFF | SPECIAL | TIME
}