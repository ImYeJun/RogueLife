using System;

[Flags]
public enum BattleEntityTrait
{
    NONE = 0,
    PLAYER = 1 << 0,
    ENEMY = 1 << 1,
    ANY = ~0
}