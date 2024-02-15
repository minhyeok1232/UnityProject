using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill_CoolTime_Script : MonoBehaviour          // 스킬을 쓰면 쿨타임이 적용되는 스크립트
{
    public List<Skill> allSkills; // 모든 스킬 객체를 저장하는 리스트
    public Dictionary<GameObject, Skill> skillEffectInstances = new Dictionary<GameObject, Skill>();

    // Script
    public SkillManager manager;                            // 모든 스킬들을 참조하기 위한 스크립트

    // GameObject
    //public Text Skill_CoolTime_Text;                 // 스킬 쿨타임 패널(UI-Text)

    public GameObject All_Panel;

    public Transform skillPanel; // 스킬 UI를 배치할 패널


    private void Awake()
    {
        
    }

    private void Start()
    {
        //All_Panel.gameObject.SetActive(false);                                    // 각 오브젝트들은 꺼준다. (복제를 해서 스킬들을 넣을 뿐, 원래 오브젝트들은 빈 스킬로 들어감)
    }
    private void Update()
    {

    }

    /******************************************************************************* 스킬 실행 순서 ***************************************************************/

    // 1단계 : 스킬 키를 누르면 스킬 이펙트가 나온다. 이 이펙트를 통해서, 해당 Skill이 무엇인지 찾아준다.

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

    // 2단계 : 해당 스킬에 대하여, Skill 클래스에 skill.CoolTimer 을 찾아서 반환한다.
    // 3단계 : 쿨타임 업데이트 함수
}