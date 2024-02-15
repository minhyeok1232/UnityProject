using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour                   // ��ų ������ �� ���¸� ����
{
    // Script
    public SkillDisplay skillDisplayPrefab;                 // SkillDisplay ��ũ��Ʈ
    public CharacterExp player;

    // Transform
    public RectTransform availableSkillsContainer;          // ��� ������ ��ų�� ǥ���� �����̳�
    public RectTransform notAvailableSkillsContainer;       // ��� �Ұ����� ��ų�� ǥ���� �����̳�
    public RectTransform TotalContainer;                    // ��밡��, �Ұ��� �����̳� : ���콺 ��ũ�ѹ� ������ ���� ����

    // GameObject
    public GameObject AvailableObject;                      // ��밡���� ��ų (Instantiate�� ����)
    public GameObject notAvailableObject;                   // ��� �Ұ����� ��ų (Instantiate�� ����)
           
    // Image
    public Image Lock;                                      // �ڹ��� (Instantiate�� ����, ��� �Ұ����� ��ų ���� ���� ����� �̹���)

    // List
    public List<Skill> allSkills;                           // ��� ��ų ������

    // Dictionary
    private Dictionary<Skill, SkillDisplay> skillDisplays = new Dictionary<Skill, SkillDisplay>();  // skillsDisplays Dictionary�� Skill�� Key��, SkillDisplay�� Value������ �޴´�.

    // SingleTone
    public static SkillManager instance;

    // Instance
    public int User_Skill_Point;                            // ó������ ��ų ����Ʈ�� 0���� �ʱ�ȭ�ȴ�.

    // Text
    public Text User_Skill_Text;                            // User_Skill_Point ��ų Text

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
        AvailableObject.gameObject.SetActive(false);                                    // �� ������Ʈ���� ���ش�. (������ �ؼ� ��ų���� ���� ��, ���� ������Ʈ���� �� ��ų�� ��)
        notAvailableObject.gameObject.SetActive(false);
    }

    // Update
    public void Update()
    {
        S_Transform(availableSkillsContainer, notAvailableSkillsContainer);             // ��밡���� ��ų���� ����, �Ұ����� ��ų���� �Ʒ��� ��ġ��Ų��. (���������� ��ȭ�� ����)
        Total_Transform(availableSkillsContainer, notAvailableSkillsContainer);         // ��� ��ų���� ��� �г��� ũ�⸦ �����ش�. 
    }
    
    // Method
    public void Skill_LevelUp(Skill skill)                                              // ��ų ������ �޼ҵ�
    {
        if (User_Skill_Point > 0)                                                       // ���� ��ų ����Ʈ�� ������,
        {
            skill.Skill_Level++;                                                        // ��ų������ 1�ø���, ���� ��ų����Ʈ�� 1���δ�.
            User_Skill_Point--;

            foreach (SkillDisplay display in FindObjectsOfType<SkillDisplay>())         // ������ ��� ��ų���� ��ȸ�Ͽ�,
            {
                if (display.skill == skill)                                             // ���� �ø� ��ų��
                {
                    display.SkillDisplayUI();                                           // ������ �÷��ش�.
                    UpdateSkillPointUI(User_Skill_Point);
                    break;
                }
            }
        }
    }

/****************************************************************** SKILL POINT ********************************************************************/

    public void LevelUp_Skill()                                                         // ���� ��ų����Ʈ�� 1 ���������ش�.
    {
        User_Skill_Point += 1;
    }

    public void UpdateSkillPointUI(int skillPoints)
    {
        User_Skill_Text.text = skillPoints.ToString();
    }
 /**************************************************************************************************************************************************/


    // ��ų ���� ������Ʈ �� UI �ݿ�
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



    /************************************************************* Update �޼ҵ� ************************************************************************/
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
            SkillDisplay display = Instantiate(skillDisplayPrefab);                     // ��� ��ų���� ��ȸ�ϸ鼭,
            display.Initialize(skill);                                                  // �� ��ų�鿡 UI�� �״�� ���󰣴�.

            skill.isUnlocked = characterLevel >= skill.requiredLevel;                   // ĳ���ͷ����� ���� ��ų�� ��� Ȱ��ȭ���°� ����ȴ�.
            display.UpdateSkillUI(skill); // UI ������Ʈ

            skillDisplays.Add(skill, display);                                          // skill(Key), display(Value) ���� ���� Dictionary�� ���� �����.

            if (!skill.isUnlocked)                                                      // ������� ���ϴ� ��ų����
            {
                display.transform.SetParent(notAvailableSkillsContainer, false);        // �����ϴ� ��ų���� �ڽĿ� ����,

                Image lockImageInstance = Instantiate(Lock, display.transform);         // display.transform �� ��ġ(�ν�����)�� Lock(�ڹ��� �̹���)�� �����Ѵ�.

                lockImageInstance.rectTransform.anchoredPosition = new Vector2(-175, 0);
                lockImageInstance.rectTransform.sizeDelta = new Vector2(80, 80);        // Lock�� ũ��� ��ġ�� �����Ѵ�.
                display.Lv_Button.interactable = false;
                // �߰������� �����ϰ� ��������
            }
            else
            {
                display.transform.SetParent(availableSkillsContainer, false);           // ��밡���� ��ų���� �ڽĿ� ����.
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