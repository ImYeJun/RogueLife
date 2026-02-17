using System;

[Flags]
public enum BattleEntityTrait
{
    PLAYER = 1 << 0,
    ENEMY = 1 << 1,
    ANY = ~0
}