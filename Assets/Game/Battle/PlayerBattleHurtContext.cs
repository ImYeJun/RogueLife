public class PlayerBattleHurtContext
{
    private int battleHealthDamage;
    private int mentalityDamage;
    private bool isOverflow;

    public PlayerBattleHurtContext(int battleHealthDamage, int mentalityDamage, bool isOverflow)
    {
        this.battleHealthDamage = battleHealthDamage;
        this.mentalityDamage = mentalityDamage;
        this.isOverflow = isOverflow;
    }

    public int BattleHealthDamage { get => battleHealthDamage; }
    public int MentalityDamage { get => mentalityDamage; }
    public bool IsOverflow { get => isOverflow; }

    public void NullifyBattleHealthDamamge() { battleHealthDamage = 0; }
    public void NullifyMentalityDamage() { mentalityDamage = 0; }
}