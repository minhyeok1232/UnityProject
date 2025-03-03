using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariousEffectsScene : MonoBehaviour
{

    public Skill_CoolTime_Script cool;
    public Skill_Instantiate skill_Instantiate;
    public Transform[] m_effects;

    public PlayerController player;

    public GameObject[] m_destroyObjects = new GameObject[10];
    GameObject gm;
    public int inputLocation;
    public static float m_gaph_scenesizefactor = 1;
    public GameObject character;
    int index = 0;

    public Skill skill;

    void Awake()
    {
        inputLocation = 0;
    }

    public void Start()
    {
        player = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    void Update()
    {
        InputKey();
    }

    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Z) && player.level >= player._skills[1].skill.requiredLevel && !player._skills[1].skill.IsCooldownActive)
        {
            index = 1;
            MakeObject();
        }
        if (Input.GetKeyDown(KeyCode.S) && player.level >= player._skills[2].skill.requiredLevel && !player._skills[2].skill.IsCooldownActive)
        {
            index = 0;
            MakeObject();
        }
        if (Input.GetKeyDown(KeyCode.D) && player.level >= player._skills[3].skill.requiredLevel && !player._skills[3].skill.IsCooldownActive)
        {
            index = 2;
            MakeObject();
        }
        if (Input.GetKeyDown(KeyCode.X) && player.level >= player._skills[4].skill.requiredLevel && !player._skills[4].skill.IsCooldownActive)
        {
            index = 3;
            MakeObject();
        }
        if (Input.GetKeyDown(KeyCode.C) && player.level >= player._skills[5].skill.requiredLevel && !player._skills[5].skill.IsCooldownActive)
        {
            index = 4;
            MakeObject();
        }

    }
    public void MakeObject()
    {
        DestroyGameObject();

        // 스킬 이펙트 인스턴스 생성
        gm = Instantiate(m_effects[index],
            m_effects[index].transform.position,
            m_effects[index].transform.rotation).gameObject;

        // 스킬 이펙트를 캐릭터의 자식으로 설정
        gm.transform.parent = character.transform;

        // 스킬 이펙트의 위치 및 크기 설정
        gm.transform.localPosition = Vector3.zero; // 캐릭터의 로컬 위치에 기반하여 이펙트의 위치를 설정합니다.
        float submit_scalefactor = m_gaph_scenesizefactor;
        if (index < 70)
            submit_scalefactor *= 0.5f;
        gm.transform.localScale = new Vector3(submit_scalefactor, submit_scalefactor, submit_scalefactor);

        // 스킬 이펙트의 방향을 캐릭터가 바라보는 방향으로 설정
        gm.transform.rotation = Quaternion.LookRotation(character.transform.forward);

        m_destroyObjects[inputLocation] = gm;
        inputLocation++;

        Skill skillToUse = cool.FindSkillByEffect(gm);
        if (skillToUse != null)
        {
            skill_Instantiate.StartCooldown(skillToUse);
        }
    }

    void DestroyGameObject()
    {
        for (int i = 0; i < inputLocation; i++)
        {
            Destroy(m_destroyObjects[i]);
        }
        inputLocation = 0;
    }
}