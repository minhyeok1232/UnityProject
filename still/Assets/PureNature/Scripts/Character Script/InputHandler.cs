using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject DeathUI;

    private MonsterBehaviour monsterBehaviour;

    private TestSlider player;
    public PlayerController cha;
    //public GameObject character;
    public Text text;
    public GameObject Clock;

    private float DeathTimer;

    public bool isDeath = false;


    private void Start()
    {
        this.enabled = false;
        player = GetComponent<TestSlider>();

        if (player != null)
        {
            player.OnPlayerDeath += HandlePlayerDeath;
        }
    }

    private void Update()
    {
        if (isDeath)
        {
            OpenDeathUI();
            TimerMethod();
        }
    }

    private void HandlePlayerDeath()
    {
        this.enabled = true;
        isDeath = true;
        DeathTimer = 5.0f;
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPlayerDeath -= HandlePlayerDeath;
        }
    }

    private void OpenDeathUI()
    {
        DeathUI.SetActive(true);
        text = GameObject.Find("Time").GetComponent<Text>();
        foreach (var monster in FindObjectsOfType<MonsterBehaviour>())
        {
            monster.DisableMonsterAttacking();
        }
        Animator animator = cha.GetComponent<Animator>();
        animator.SetBool("Restore", false);
    }

    public void CloseDeathUI()
    {
        DeathUI.SetActive(false);
        isDeath = false;
        cha.ResetCharacterPosition();
        cha.isDead = false;
        player.RestoreHealthAndMana();
        Animator animator = cha.GetComponent<Animator>();
        animator.SetBool("Die", false);
    }

    private void TimerMethod()
    {
        if (DeathTimer > 0)
        {
            ClockAnimationHandler clockHelper = Clock.GetComponent<ClockAnimationHandler>();
            if (clockHelper != null)
            {
                clockHelper.StartClockAnimation();
            }

            DeathTimer -= Time.deltaTime;
            text.text = Mathf.CeilToInt(DeathTimer).ToString();
        }
        else
        {
            isDeath = false;
            CloseDeathUI();
        }
    }
}