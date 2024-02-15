using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalManager : MonoBehaviour
{
    // Scipt
    public static CriticalManager instance;             // CriticalManager 클래스의 인스턴스를 전역적으로 접근할 수 있는 정적 변수를 선언

    // instance
    public float baseCriticalChance = 0.1f;             // 기본 크리티컬 확률
    private float currentCriticalChance;                // 현재 크리티컬 확률 (무언가에 따라, 크리티컬확률이 증가할수가 있음)

    // Awake
    private void Awake()
    {
        if(instance == null)                            // 싱글톤을 사용해서, 메모리 절약
        {                                               
            instance = this;
        }
    }

    private void Start()
    {
        currentCriticalChance = baseCriticalChance;     // 캐릭터의 레벨업 또는 스탯 요인으로 크리티컬 확률을 올려 줄 수 있는 수단으로
    }                                                   // 변수하나를 더 넣어주었다.

    public bool CheckCritical()                         // True/False 값만 나오게 됨
    {
        return Random.value <= currentCriticalChance;   // 0~1사이의 랜덤한 값을 0.1f라는 수치랑 같을 때
    }

    public void IncreaseCriticalChance(float amount)    // amount 값에 따라, 동적으로 크리티컬확률을 증가시킬 수 있다.
    {
        currentCriticalChance += amount;
    }
}

