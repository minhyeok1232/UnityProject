using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class CharacterExp : MonoBehaviour
{
    // Script
    public SkillManager manager;            // SkillManager
    public PlayerController player;         // PlayerController
    public TestSlider testSlider;           // TestSlider
    public MainScript main;                 // MainScript
    public SkillDisplay display;            // SkillDisplay

    // Class
    public Skill skill;                     // Skill

    // List
    public List<Skill> allSkills;           // 모든 스킬을 담고 있는 리스트

    // GameObject
    public GameObject character;            // 플레이어
    public ParticleSystem levelupeffect;    // 레벨업 이펙트
    public GameObject leveluptext;          // 레벨업 텍스트

    // Instance
    public int request = 10;                // 처음의 경험치 요구량
    public int currentLevel = 1;            // 처음의 레벨
    public int currentExp;                  // 현재 경험치

    // Awake
    private void Awake()
    {
        manager = GameObject.Find("Skill_use_Panel").GetComponent<SkillManager>();  // Start에서 CloseSkill하기 전에, 활성화 되어있는 스킬들을 할당

        testSlider = GameObject.FindObjectOfType<TestSlider>();                     // TestSlider 스크립트가 들어가있는 게임오브젝트
        main = GameObject.FindObjectOfType<MainScript>();                           // MainScript 스크립트가 들어가있는 게임 오브젝트
    }

    // Start
    public void Start()
    {
        player.CloseSkill();                                                        // 스킬창을 닫아준다.

        GameManagerScript.Instance.exp = this;                                      // DontDestroy 싱글톤으로 되어있는 GameManagerScript 할당
        //GameManagerScript.Instance.LoadGameState();                               // 게임을 시작할 때 LoadGameState 
        bool newQuestStarted = main.CheckQuestRequirementsByLevel();
 
    }

    // Update
    private void Update()
    {
        if (currentExp >= request)                                                  // 현재 가지고 있는 경험치가 요구 경험치보다 많게 되면, 레벨업
            LevelUp();

        testSlider.UpdateSlider();                                                  // 새로운 경험치 갱신을 한다.
        player.level = currentLevel;// 레벨 달성할 때 마다 새로운 퀘스트가 들어오는지 확인
    }

    // Method
    public void MonsterKillFunction(MonsterInformation.Monster monster)             // CSV파일에 있는 해당몬스터의 경험치는 현재경험치에 더해서 갱신한다.
    {
        currentExp += monster.Exp;
    }

    public void LevelUp()
    {   
        currentExp -= request;                                                      // 레벨은 오르고, 경험치는 변동
        request = (int)(request * 1.15);                                            // 레벨업할 때마다 경험치는 1.15배씩 증가
        currentLevel++;

        bool newQuestStarted = main.CheckQuestRequirementsByLevel();                // 새로운 퀘스트를 받을 수 있는지 모든 퀘스트를 순회하여 검사
        if (newQuestStarted)                                                        // 새로 받을 수 있는 퀘스트가 존재한다면,
        {
            Debug.Log("새로운 퀘스트 추가");
        }

        LV_Effect();                                                                // 레벨업 이펙트
        manager.LevelUp_Skill();                                                    // 찍을 수 있는 스킬포인트 1 증가
        manager.UpdateSkillPointUI(manager.User_Skill_Point);                       // 이것을 Text 형태로 보여줄 수 있게 하는 메소드
        
        CheckForUnlockedSkills();                                                   // 레벨업하면서, 스킬의 잠금여부를 확인하는 메소드
        foreach (var skill in allSkills) // 모든 스킬에 대해
        {
            // 스킬 UI를 업데이트합니다.
            display.UpdateSkillUI(skill);
            if(!skill.isUnlocked)
            {
                display.Lv_Button.interactable = false;
            }
            else
            {
                display.Lv_Button.interactable = skill.Skill_Level < skill.maxLevel;
            }
        }
        player.level = currentLevel;
    }

    public void CheckForUnlockedSkills()
    {
        foreach (var skill in allSkills)                                    // 모든 스킬을 순항하면서,
        {
            bool isUnlocked = currentLevel >= skill.requiredLevel;
            manager.UpdateSkillAvailability(skill, isUnlocked);                     // 스킬을 사용할 수 있는지, 없는지 확인을 한다. (Skill 객체와, bool형태를 받아서 반환)
        }
    }

    private void LV_Effect()                                                        // 레벨업 이펙트(Particle) 을 보여주는 메소드
    {
        levelupeffect.Play();
        Instantiate(leveluptext, levelupeffect.transform.position, levelupeffect.transform.rotation);
    }

 /****************************************************************** LOAD DATA *****************************************************************/
    public void LoadLevel(int loadedLevel)                                          // 레벨 로드 (Load Data 데이터 로드)
    {
        currentLevel = loadedLevel;

        for (int i = 1; i < loadedLevel; i++)
        {
            request = (int)(request * 1.15);
        }
    }
    public void LoadExp(int loadedexp)
    {
        currentExp = loadedexp;
    }
/****************************************************************** LOAD DATA *****************************************************************/
}