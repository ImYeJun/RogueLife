using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleSkeletonRule", menuName = "Scriptable Objects/ScheudleRule/ScheduleSkeletonRule")]
public class ScheduleSkeletonRule : ScriptableObject
{
    [SerializeField] private int minLayer = 0;
    [SerializeField] private int maxLayer = 0;
    [SerializeField] private int minNodePerLayer = 0;
    [SerializeField] private int maxNodePerLayer = 0;
    [SerializeField] private int maxNodeLinkCount = 0;
    [SerializeField] private float additionalLinkMultiplierChance = 0;

    public ScheduleSkeletonRule(int minLayer, int maxLayer, int minNodePerLayer, int maxNodePerLayer, int maxNodeLinkCount, float additionalLinkMultiplierChance)
    {
        this.minLayer = minLayer;
        this.maxLayer = maxLayer;
        this.minNodePerLayer = minNodePerLayer;
        this.maxNodePerLayer = maxNodePerLayer;
        this.maxNodeLinkCount = maxNodeLinkCount;
        this.additionalLinkMultiplierChance = additionalLinkMultiplierChance;
    }

    public int MinLayer { get => minLayer; }
    public int MaxLayer { get => maxLayer; }
    public int MinNodePerLayer { get => minNodePerLayer; }
    public int MaxNodePerLayer { get => maxNodePerLayer; }
    public int MaxNodeLinkCount { get => maxNodeLinkCount; }
    public float AdditionalLinkMultiplierChance { get => additionalLinkMultiplierChance; }
}