using System;
using System.Runtime.InteropServices;

[Flags]
public enum BattleEntityCondition
{
    NONE = 0,
    STUNNED = 1 << 0,
    ANY = ~0
}