using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Scriptable Objects/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject, IRunDiaryEnemyDatabaseContext {
    [SerializeField] List<EnemyData> availableEnemies;
    public List<EnemyData> AvailableEnemies => availableEnemies;
}