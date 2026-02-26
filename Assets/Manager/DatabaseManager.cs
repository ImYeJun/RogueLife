using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] private SpecialDiaryDatabase specialDiaryDatabase;
    [SerializeField] private ScheduleDatabase scheduleDatabase;
    [SerializeField] private CardDatabase cardDatabase;
    [SerializeField] private BelongingsDatabase belongingsDatabase;
    [SerializeField] private EnemyDatabase enemyDatabase;
    [SerializeField] private BattleStatusEffectDatabase battleStatusEffectDatabase;
    [SerializeField] private IncidentDatabase incidentDatabase;
    [SerializeField] private TransactionChoiceDatabase transactionChoiceDatabase;

    public (
            BelongingsDatabase belongingsDatabase, 
            CardDatabase cardDatabase, 
            EnemyDatabase enemyDatabase, 
            IncidentDatabase incidentDatabase, 
            ScheduleDatabase scheduleDatabase, 
            SpecialDiaryDatabase specialDiaryDatabase, 
            TransactionChoiceDatabase transactionChoiceDatabase, 
            BattleStatusEffectDatabase battleStatusEffectDatabase
            ) Databaes { get => (belongingsDatabase, cardDatabase, enemyDatabase, incidentDatabase, scheduleDatabase, specialDiaryDatabase, transactionChoiceDatabase, battleStatusEffectDatabase ); }
}
