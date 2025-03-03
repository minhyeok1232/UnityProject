using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill_CoolTime_Script : MonoBehaviour          // ��ų�� ���� ��Ÿ���� ����Ǵ� ��ũ��Ʈ
{
    public List<Skill> allSkills; // ��� ��ų ��ü�� �����ϴ� ����Ʈ
    public Dictionary<GameObject, Skill> skillEffectInstances = new Dictionary<GameObject, Skill>();

    // Script
    public SkillManager manager;                            // ��� ��ų���� �����ϱ� ���� ��ũ��Ʈ

    // GameObject
    //public Text Skill_CoolTime_Text;                 // ��ų ��Ÿ�� �г�(UI-Text)

    public GameObject All_Panel;

    public Transform skillPanel; // ��ų UI�� ��ġ�� �г�


    private void Awake()
    {
        
    }

    private void Start()
    {
        //All_Panel.gameObject.SetActive(false);                                    // �� ������Ʈ���� ���ش�. (������ �ؼ� ��ų���� ���� ��, ���� ������Ʈ���� �� ��ų�� ��)
    }
    private void Update()
    {

    }

    /******************************************************************************* ��ų ���� ���� ***************************************************************/

    // 1�ܰ� : ��ų Ű�� ������ ��ų ����Ʈ�� ���´�. �� ����Ʈ�� ���ؼ�, �ش� Skill�� �������� ã���ش�.

    public Skill FindSkillByEffect(GameObject s)
    {
        string effectName = GetOriginalPrefabName(s);
        foreach (Skill skill in allSkills)
        {
            if (skill.skill_Effects != null && GetOriginalPrefabName(skill.skill_Effects) == effectName)
            {
                return skill;
            }
        }
        return null; 
    }
    public string GetOriginalPrefabName(GameObject instance)
    {
        return instance.name.Replace("(Clone)", "").Trim();
    }

    // 2�ܰ� : �ش� ��ų�� ���Ͽ�, Skill Ŭ������ skill.CoolTimer �� ã�Ƽ� ��ȯ�Ѵ�.
    // 3�ܰ� : ��Ÿ�� ������Ʈ �Լ�
}