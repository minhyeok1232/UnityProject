using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TestSlider : MonoBehaviour
{
    // Singletone
    public static TestSlider instance;

    // Deligate
    public delegate void PlayerDeathEventHandler();
    public event PlayerDeathEventHandler OnPlayerDeath;

    // Script
    public MonsterInformation monsterInformation;                       // 각 Monster의 Damage 를 받기 위한 Script
    public CharacterExp characterExp;                                   // Level up 을 위한 CharacterExp Script 참조
    public PlayerController player;

    public Slider slHP;                                                 // Hp = SlHp
    public Slider slMP;                                                 // Mp = SlMp
    public Slider slExp;                                                // Exp = SlExp

    public Text HPText;                                                 // HpText (슬라이더 위에 나타내줄 텍스트)
    public Text MPText;                                                 // MPText
    public Text EXPText;                                                // ExpText
    public Text level;                                                  // level (캐릭터의 레벨을 표시)

    // Property
    public int PlayerHealth                                             // 새로운 PlayerHealth 프로퍼티 추가
    {
        get => playerHealth;
        set
        {
            playerHealth = Mathf.Clamp(value, 0, maxHealth);            // 체력이 0 이하, maxHealth 이상으로 설정되는 것을 방지
            UpdateSlider();                                             // playerHealth 값을 받아, playerHealth을 받아서, UI에있는 Slider값을 Update해주고,
        }
    }                                                                   // 플레이어의 체력의 변화에 따른 체력을 나타내주는, Text UI를 PlayerHealth 프로퍼티로 정의를 한다.


    public int playerHealth = 300;                                     // PlayerController에서 playerHealth 를 관리하지않고, TestSlider에서 관리한다.

    public int PlayerMana
    {
        get => playerMana;
        set
        {
            playerMana = Mathf.Clamp(value, 0, maxMana);
            UpdateSlider();
        }
    }

    public int playerMana = 100;                                       // 내 캐릭터의 Mp (값이 변한다.)

    public int maxHealth = 300;                                        // 내 캐릭터의 최대 Hp (일정 레벨에 따라 값은 고정)
    public int maxMana = 100;                                          // 내 캐릭터의 최대 Mp (일정 레벨에 따라 값은 고정)


    private Animator animator;                      // 캐릭터에 추가한 컴퍼넌트 애니메이터 : animator

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        GameManagerScript.Instance.test = this;


        slHP.value = 1.0f;
        slMP.value = 1.0f;
        HPText.color = Color.yellow;
        MPText.color = Color.yellow;
        EXPText.color = Color.white;
        level.color = Color.white;
        HPText.text = "300/300";
        MPText.text = "100/100";
        level.text = "Level" + characterExp.currentLevel;
        monsterInformation = GetComponent<MonsterInformation>();        // monsterInformation 은 현재 스크립트에서 MonsterInformation Component가 필요하다.
        characterExp = GameObject.FindObjectOfType<CharacterExp>(); PlayerController player = GetComponent<PlayerController>();
        player = GetComponent<PlayerController>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }

    public void UpdateSlider()
    {
        slHP.value = (float)playerHealth / maxHealth;
        slMP.value = (float)playerMana / maxMana;
        slExp.value = (float)characterExp.currentExp / characterExp.request;
        HPText.text = $"{playerHealth} / {maxHealth}";
        MPText.text = $"{playerMana} / {maxMana}";
        EXPText.text = $"{characterExp.currentExp}/{characterExp.request}";
        level.text = "Lv. " + $"{characterExp.currentLevel}";
    }

    public void RestoreHealthAndMana()
    {
        PlayerHealth = maxHealth;
        PlayerMana = maxMana;
    }

    public void IncreasedValueHealth(int healthValue)
    {
        PlayerHealth += healthValue;
    }

    public void PlayerDie()
    {
        animator.SetBool("Die", true);

        if (player.isDead) return;

        player.isDead = true;

        if (OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }

    public void LoadHp(int loadedHp)
    {
        PlayerHealth = loadedHp;
    }
    public void LoadMp(int loadedMp)
    {
        PlayerMana = loadedMp;
    }

}