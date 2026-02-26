using UnityEngine;

public class SkeletonGenerateRuleManager : MonoBehaviour
{
    [SerializeField] private ScheduleSkeletonRule scheduleSkeletonRule;
    [SerializeField] private ScheduleNodeTypeResolveRule scheduleNodeTypeResolveRule;

    public (ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule typeResolveRule) Rules { get => (scheduleSkeletonRule, scheduleNodeTypeResolveRule); }
}