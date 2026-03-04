using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameRunManager
{
#if UNITY_EDITOR

    [HideInInspector] 
    [SerializeField] private List<BelongingsEntity> testBelongingsEntities = new List<BelongingsEntity>();
    [SerializeField] private List<CardEntity> testCardEntities = new List<CardEntity>();
    [SerializeField] private int testHurtDamage;
    [SerializeField] private bool isOverflowable;

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

        Debug.Log($"플레이어에게 \"{testHurtDamage}\" 만큼의 데미지 가격");
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