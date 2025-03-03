using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour
{
    // Script
    public Skill skill;                         // �� ��ų ���÷��̰� ǥ���� ��ų ��ü
    public SkillManager skillManager;           // SkillManager ��ũ��Ʈ

    // UI
    public Image skillImage;                    // ��ų �������� ǥ���� �̹���
    public Text skillNameText;                  // ��ų �̸��� ǥ���� �ؽ�Ʈ
    public Text skillLevelText;                 // ��ų ������ ǥ���� �ؽ�Ʈ

    public Button Lv_Button;                    // ��ų ������ ��ư

    // Start
    public void Start()
    {
        Lv_Button.onClick.AddListener(() => SkillManager.instance.Skill_LevelUp(skill));    // �̱��� ��ų�Ŵ��� �������� ���� skill ��ü�� ���Ͽ�, ��ų������ �޼ҵ带 �ش�.
    }                                                                                       // �����ʸ� ���ؼ�

    // Method
    public void SkillDisplayUI()
    {
        skillLevelText.text = skill.Skill_Level.ToString();                     // ��ų�� ������ �ؽ�Ʈ�� ǥ�ð� �ȴ�.
        Lv_Button.gameObject.SetActive(skill.Skill_Level < skill.maxLevel);     // ��ų�� �ִ�ġ ������ �޼��� �� ��ư�� ������ �ʰ� �ȴ�.
    }

    public void UpdateSkillUI(Skill skill)                                      // ��ų�� ��� �����̸�, ��ȭ���� ���ϰ� �ϴ� ����
    {
        skillLevelText.text = skill.Skill_Level.ToString();
        skillManager.Lock.gameObject.SetActive(!skill.isUnlocked);              // ��� �̹����� ���� ���
    }

    /************************************************************* ��ų �����ͷ� UI ���� ***********************************************************************/
    public void Initialize(Skill skillData)                                     // ��ų �����ͷ� UI�� �ʱ�ȭ �ϴ� �޼ҵ�
    {
        skill = skillData;
        UpdateSkillDisplay();
    }

    public void UpdateSkillDisplay()                                            // ������ UI ������Ʈ�� ������Ʈ�ϴ� �޼ҵ�
    {
        skillImage.sprite = skill.icon;
        skillNameText.text = skill.skillName;
        skillLevelText.text = skill.Skill_Level.ToString();
    }
/***********************************************************************************************************************************************************/
}
