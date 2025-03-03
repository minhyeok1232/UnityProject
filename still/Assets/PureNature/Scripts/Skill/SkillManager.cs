using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour                   // 스킬 데이터 및 상태를 관리
{
    // Script
    public SkillDisplay skillDisplayPrefab;                 // SkillDisplay 스크립트
    public CharacterExp player;

    // Transform
    public RectTransform availableSkillsContainer;          // 사용 가능한 스킬을 표시할 컨테이너
    public RectTransform notAvailableSkillsContainer;       // 사용 불가능한 스킬을 표시할 컨테이너
    public RectTransform TotalContainer;                    // 사용가능, 불가능 컨테이너 : 마우스 스크롤바 내리기 위한 범위

    // GameObject
    public GameObject AvailableObject;                      // 사용가능한 스킬 (Instantiate로 복사)
    public GameObject notAvailableObject;                   // 사용 불가능한 스킬 (Instantiate로 복사)
           
    // Image
    public Image Lock;                                      // 자물쇠 (Instantiate로 복사, 사용 불가능한 스킬 위에 덮어 씌우는 이미지)

    // List
    public List<Skill> allSkills;                           // 모든 스킬 데이터

    // Dictionary
    private Dictionary<Skill, SkillDisplay> skillDisplays = new Dictionary<Skill, SkillDisplay>();  // skillsDisplays Dictionary는 Skill를 Key값, SkillDisplay를 Value값으로 받는다.

    // SingleTone
    public static SkillManager instance;

    // Instance
    public int User_Skill_Point;                            // 처음에는 스킬 포인트가 0으로 초기화된다.

    // Text
    public Text User_Skill_Text;                            // User_Skill_Point 스킬 Text

    // Awake
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start
    private void Start()
    {   
        AvailableObject.gameObject.SetActive(false);                                    // 각 오브젝트들은 꺼준다. (복제를 해서 스킬들을 넣을 뿐, 원래 오브젝트들은 빈 스킬로 들어감)
        notAvailableObject.gameObject.SetActive(false);
    }

    // Update
    public void Update()
    {
        S_Transform(availableSkillsContainer, notAvailableSkillsContainer);             // 사용가능한 스킬들은 위에, 불가능한 스킬들은 아래에 위치시킨다. (레벨업마다 변화가 있음)
        Total_Transform(availableSkillsContainer, notAvailableSkillsContainer);         // 모든 스킬들을 담는 패널의 크기를 정해준다. 
    }
    
    // Method
    public void Skill_LevelUp(Skill skill)                                              // 스킬 레벨업 메소드
    {
        if (User_Skill_Point > 0)                                                       // 남은 스킬 포인트가 있으면,
        {
            skill.Skill_Level++;                                                        // 스킬레벨을 1올리고, 남은 스킬포인트를 1줄인다.
            User_Skill_Point--;

            foreach (SkillDisplay display in FindObjectsOfType<SkillDisplay>())         // 복제된 모든 스킬들을 순회하여,
            {
                if (display.skill == skill)                                             // 내가 올린 스킬에
                {
                    display.SkillDisplayUI();                                           // 레벨을 올려준다.
                    UpdateSkillPointUI(User_Skill_Point);
                    break;
                }
            }
        }
    }

/****************************************************************** SKILL POINT ********************************************************************/

    public void LevelUp_Skill()                                                         // 남은 스킬포인트를 1 증가시켜준다.
    {
        User_Skill_Point += 1;
    }

    public void UpdateSkillPointUI(int skillPoints)
    {
        User_Skill_Text.text = skillPoints.ToString();
    }
 /**************************************************************************************************************************************************/


    // 스킬 상태 업데이트 및 UI 반영
    public void UpdateSkillAvailability(Skill skill, bool isUnlocked)
    {
        if (skillDisplays.TryGetValue(skill, out SkillDisplay display))
        {
            skill.isUnlocked = isUnlocked;

            if (isUnlocked)
            {
                display.transform.SetParent(availableSkillsContainer, false);
                display.Lv_Button.interactable = true;

                var lockImages = display.GetComponentsInChildren<Image>();
                foreach (var lockImage in lockImages)
                {
                    if (lockImage.gameObject.name.Contains("Lock"))
                    {
                        Destroy(lockImage.gameObject);
                    }
                }
            }
            else
            {
                display.transform.SetParent(notAvailableSkillsContainer, false);
            }
        }
    }



    /************************************************************* Update 메소드 ************************************************************************/
    public void S_Transform(RectTransform availablePanel, RectTransform notAvailablePanel)
    {
        Vector3 availablePanelBottom = GetBottomPosition(availablePanel);
        notAvailablePanel.localPosition = new Vector3(notAvailablePanel.localPosition.x, availablePanelBottom.y, notAvailablePanel.localPosition.z);
    }
    Vector3 GetBottomPosition(RectTransform rectTransform)
    {
        return new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - rectTransform.rect.height, rectTransform.localPosition.z);
    }

    public void Total_Transform(RectTransform availablePanel, RectTransform notAvailablePanel)
    {
        float totalHeight = availablePanel.rect.height + notAvailablePanel.rect.height;
        TotalContainer.sizeDelta = new Vector2(availablePanel.rect.width, totalHeight);
    }

    public void Level_Setting()
    {
        int characterLevel = player.currentLevel;

        foreach (var skill in allSkills)
        {
            SkillDisplay display = Instantiate(skillDisplayPrefab);                     // 모든 스킬들을 순회하면서,
            display.Initialize(skill);                                                  // 각 스킬들에 UI가 그대로 따라간다.

            skill.isUnlocked = characterLevel >= skill.requiredLevel;                   // 캐릭터레벨에 따라 스킬의 잠금 활성화상태가 변경된다.
            display.UpdateSkillUI(skill); // UI 업데이트

            skillDisplays.Add(skill, display);                                          // skill(Key), display(Value) 값을 가진 Dictionary를 새로 만든다.

            if (!skill.isUnlocked)                                                      // 사용하지 못하는 스킬들은
            {
                display.transform.SetParent(notAvailableSkillsContainer, false);        // 사용못하는 스킬들의 자식에 들어가며,

                Image lockImageInstance = Instantiate(Lock, display.transform);         // display.transform 의 위치(인스펙터)에 Lock(자물쇠 이미지)를 생성한다.

                lockImageInstance.rectTransform.anchoredPosition = new Vector2(-175, 0);
                lockImageInstance.rectTransform.sizeDelta = new Vector2(80, 80);        // Lock의 크기와 위치를 조정한다.
                display.Lv_Button.interactable = false;
                // 추가적으로 사용못하게 만들어야함
            }
            else
            {
                display.transform.SetParent(availableSkillsContainer, false);           // 사용가능한 스킬들의 자식에 들어간다.
            }
        }
    }

 /****************************************************************************************************************************************************/

 /****************************************************************** LOAD DATA ***********************************************************************/
    public void LoadSkillPoints(int points)
    {
        User_Skill_Point = points;
        UpdateSkillPointUI(points);
    }
/******************************************************************* LOAD DATA ***********************************************************************/
}