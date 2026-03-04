using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameRunManager
{
#if UNITY_EDITOR

    [HideInInspector] 
    [SerializeField] private List<BelongingsEntity> testBelongingsEntities = new List<BelongingsEntity>();
    [SerializeField] private List<CardEntity> testCardEntities = new List<CardEntity>();
    
    [Header("Hurt Test Settings")]
    [SerializeField] private int testHurtDamage;
    [SerializeField] private bool isOverflowable;

    [Header("Heal Test Settings")]
    [SerializeField] private int testHealAmount;
    [SerializeField] private bool isHealOverflowable;

    public void TestAddBelongings()
    {
        if (!CheckGameRunExsited()) { return; }

        foreach (var entity in testBelongingsEntities)
        {
            CurrentRun.TestAddBelongings(entity);
        }

        Debug.Log("테스트 소지품 지급 완료!");
    }

    public void TestAddCard()
    {
        if (!CheckGameRunExsited()) { return; }

        foreach (var entity in testCardEntities)
        {
            CurrentRun.TestAddCard(entity);
        }

        Debug.Log("테스트 카드 지급 완료!");
    }

    public void TestHurtPlayer()
    {
        if (!CheckGameRunExsited()) { return; }
        
        CurrentRun.TestHurtPlayer(testHurtDamage, isOverflowable);

        Debug.Log($"플레이어에게 \"{testHurtDamage}\" 만큼의 데미지 가격 (초과 데미지 적용: {isOverflowable})");
    }

    public void TestHealMentality()
    {
        if (!CheckGameRunExsited()) { return; }

        CurrentRun.TestHealMentality(testHealAmount, isHealOverflowable);
        Debug.Log($"플레이어의 멘탈을 \"{testHealAmount}\" 만큼 회복 (초과 회복 적용: {isHealOverflowable})");
    }

    public void TestHealBattleHealth()
    {
        if (!CheckGameRunExsited()) { return; }

        CurrentRun.TestHealBattleHealth(testHealAmount);
        Debug.Log($"플레이어의 전투 체력을 \"{testHealAmount}\" 만큼 회복");
    }

    private bool CheckGameRunExsited()
    {
        if (CurrentRun == null)
        {
            Debug.LogWarning("CurrentRun이 존재 하지 않습니다.");
            return false;
        }

        return true;
    }
#endif
}