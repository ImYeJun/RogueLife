using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleData", menuName = "Scriptable Objects/ScheduleData")]
public class ScheduleData : ScriptableObject
{
    [SerializeField] private string scheduleName;
    [SerializeField] private string description;
    [SerializeField] private Sprite[] usualBackground;
    [SerializeField] private Sprite battleBackground;
    [SerializeField] private AudioClip usualBGM;
    [SerializeField] private AudioClip battleBGM;
    [SerializeField] private Sprite choiceSprite;
    [SerializeField] private EnemyData bossData;

    public string ScheduleName { get => scheduleName; }
    public string Description { get => description; }
    public Sprite[] UsualBackground { get => usualBackground; }
    public Sprite BattleBackground { get => battleBackground; }
    public AudioClip UsualBGM { get => usualBGM; }
    public AudioClip BattleBGM { get => battleBGM; }
    public Sprite ChoiceSprite { get => choiceSprite; }
    public EnemyData BossData { get => bossData; }
}
