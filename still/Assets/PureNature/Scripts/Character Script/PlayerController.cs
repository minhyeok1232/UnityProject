using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Skill_Instantiate[] _skills;

    // Script
    public MainScript mainScript;                   // NPC들의 대화시스템을 관리하는 MainScript를 참조
    public MonsterInformation monsterInformation;   // MonsterInformation 정보를 담는 Script를 참조
    public SkillSlot[] skillSlots;

    // Component
    private Rigidbody rb;                           // 캐릭터에 추가한 컴퍼넌트 리지드바디 : rb
    private Animator animator;                      // 캐릭터에 추가한 컴퍼넌트 애니메이터 : animator

    // Instance
    public float rotationSpeed = 5f;                // 회전하는 속도
    public float Speed = 4f;                        // 캐릭터가 걷는 속도
    private int comboCount = 0;                     // 콤보공격을 위한 변수는 0으로 초기화 한다.

    // Boolean
    private bool isGrounded = true;                 // 땅에 닿아있는 상태 : 이를 판단하여, 캐릭터의 점프 여부를 판단할 수 있다.
    public bool isDead = false;                     // 캐릭터의 사망여부 판단
    public bool canAttack = true;
    public bool canAttack2 = false;
    public static bool Skill_Is_Activated = false;

    public GameObject _hitbox; // 히트박스 오브젝트에 대한 참조

    [SerializeField]
    private GameObject Skill_On;

    // Vector3
    private Vector3 initialPosition;
    public Vector3 lastposition;

    public int level = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();             // 캐릭터는 중력, 애니메이션 등의 기본적인 Component 를 가지고 있다.
        animator = GetComponent<Animator>();

        mainScript = GameObject.Find("Main").GetComponent<MainScript>();    // Main 이라는 GameObject에 추가된 Component인 MainScript Script를 참조한다.
        monsterInformation = GetComponent<MonsterInformation>();

        initialPosition = gameObject.transform.position;
    }

    public void Start()
    {
        GameManagerScript.Instance.player = this;
        GameManagerScript.Instance.LoadGameState();
    }

    // Update
    void Update()
    {
        lastposition = transform.position;

        if (!isDead)
        {
            HandleInput();
        }

        TryOpenSkillSlot();

        HitBoxAttack();

        for (int i = 0; i < _skills.Length; i++)
        {
            if (level < _skills[i].skill.requiredLevel)
            {
                _skills[i].gameObject.SetActive(false);
            }
            else 
            {
                _skills[i].gameObject.SetActive(true);
            }
        }

    }

    // Method
    public void HandleInput()
    {
        float horizontal = 0f; // 좌, 우 키에 대한 입력
        float vertical = 0f;   // 상, 하 키에 대한 입력

        if (Input.GetKey(KeyCode.RightArrow))
            horizontal = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            horizontal = -1f;

        if (Input.GetKey(KeyCode.UpArrow))
            vertical = 1f;
        else if (Input.GetKey(KeyCode.DownArrow))
            vertical = -1f;

        var moveDirection = new Vector3(horizontal, 0f, vertical);          // X와 Z축으로만 움직이기 때문에, Vector3 형태의 새로운 객체는 y값을 0으로 받고, 이를 moveDirection 으로 정의한다.
        moveDirection.Normalize();                                          // X, Y, Z의 벡터(플레이어가 움직이는 방향)을 -1.0f ~ 1.0f 으로 정규화 한다.

        if (moveDirection.magnitude > 0)                                    // movdeDirection.magnitude는 벡터의 크기를 나타내며, 각 정규화된 벡터의 X,Y,Z 제곱값 루트이다.
        {
            var targetrotation = Quaternion.LookRotation(moveDirection);    // 주어진 moveDirection 벡터를 바라보는 회전을 나타내는 쿼터니언 계산
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, Time.deltaTime * rotationSpeed);  // 현재 바라보는 방향에서 targetrotation방향으로, rotationSpeed만큼 회전한다.
                                                                                                                        // 이는 더욱 부드럽게 회전시켜주는 Slerp 옵션이다.
            rb.MovePosition(rb.position + moveDirection * Speed * Time.deltaTime);  // 현재 캐릭터가 moveDirection 방향으로 이동하며, 주어진 Speed만큼 속도를 낸다.
                                                                                    // Time.deltaTime은 매 프레임마다 시간이 증가하는 옵션으로 추가해주며, 현재 rb Component를 추가한 캐릭터의 위치값을 받아와 더해준다.
            if (Input.GetKey(KeyCode.LeftShift))                            // LeftShift Key를 누르면 빠른걸음을 시키는 애니메이션 추가. 즉 누르고 있을 때, MoveSpeed의 값을 증가
                animator.SetFloat("MoveSpeed", 1.0f);                       // MoveSpeed : 1.0f
            else
                animator.SetFloat("MoveSpeed", 0.5f);                       // MoveSpeed : 0.5f
        }
        else
            animator.SetFloat("MoveSpeed", 0f);                             // moveDirection.magnitude 값이 0이라면, 정규화된 벡터들의 값이 전부 0이기 때문에 이동하지 않는다는 뜻. 즉, MoveSpeed : 0.0f     

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)   // Space 키를 누르면
        {
            isGrounded = false;
            animator.SetBool("IsJumping", true);
            StartCoroutine(DuringJumping(0.8f));  // 점프하는 동안의 코루틴 호출
        }

        if (Input.GetKeyDown(KeyCode.A) && canAttack && canAttack2)           // NPC랑 대화중일 때 (NEXT UI)를 클릭해도 공격모션이 발동되어, 이를 방지하기 위해 NPC랑대화중이 아닌 상태에서 마우스 클릭시
        {
            StartCoroutine(ComboAttack());                                  // StopCoroutine을 한 이후, StartCoroutine을 한다.
        }
    }
    public void TryOpenSkillSlot()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!Skill_Is_Activated)
            {
                OpenSkill();
            }
            else
            {
                CloseSkill();
            }
        }
    }

    public void HitBoxAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && level >= _skills[1].skill.requiredLevel && !_skills[1].skill.IsCooldownActive)
        {
            _hitbox.tag = "atk_z"; // 히트박스의 태그 변경
            _hitbox.transform.localScale = new Vector3(4, 4, 4); // 히트박스의 크기 설정
            _hitbox.SetActive(true); // 히트박스 활성화

            StartCoroutine(DisableHitboxAfterDelay(2)); // 2초 후 히트박스 비활성화/
        }
        if (Input.GetKeyDown(KeyCode.S) && level >= _skills[2].skill.requiredLevel && !_skills[2].skill.IsCooldownActive)
        {
            _hitbox.tag = "atk_v";
            _hitbox.transform.localScale = new Vector3(7, 7, 7);
            Vector3 newPosition = transform.position + transform.forward * 10;
            _hitbox.transform.position = newPosition;
            _hitbox.SetActive(true); // 히트박스 활성화

            StartCoroutine(DisableHitboxAfterDelay(2)); // 2초 후 히트박스 비활성화/
        }
        if (Input.GetKeyDown(KeyCode.D) && level >= _skills[3].skill.requiredLevel && !_skills[3].skill.IsCooldownActive)
        {
            _hitbox.tag = "atk_d";
            _hitbox.transform.localScale = new Vector3(11, 11, 11);
            _hitbox.SetActive(true); // 히트박스 활성화
            StartCoroutine(DisableHitboxAfterDelay(3));
        }
        if (Input.GetKeyDown(KeyCode.X) && level >= _skills[4].skill.requiredLevel && !_skills[4].skill.IsCooldownActive)
        {
            _hitbox.tag = "atk_x";
            _hitbox.transform.localScale = new Vector3(13.5f, 13.5f, 13.5f);
            _hitbox.SetActive(true); // 히트박스 활성화
            StartCoroutine(DisableHitboxAfterDelay(3));

        }
        if (Input.GetKeyDown(KeyCode.C) && level >= _skills[5].skill.requiredLevel && !_skills[5].skill.IsCooldownActive)
        {
            _hitbox.tag = "atk_c";
            _hitbox.transform.localScale = new Vector3(20f, 20f, 20f);
            _hitbox.SetActive(true); // 히트박스 활성화
            StartCoroutine(DisableHitboxAfterDelay(7));

        }
    }
    IEnumerator DisableHitboxAfterDelay(float delay)

    {
        yield return new WaitForSeconds(delay);
        _hitbox.SetActive(false); // 히트박스 비활성화
    }

    private void OpenSkill()
    {
        Skill_On.gameObject.SetActive(true);
        Skill_Is_Activated = true;
    }

    public void CloseSkill()
    {
        Skill_On.gameObject.SetActive(false);
        Skill_Is_Activated = false;
    }

    // Coroutine
    IEnumerator DuringJumping(float duration)                               // float형 변수인 duration은 0.8f로 선언했기에,
    {
        yield return new WaitForSeconds(duration);                          // 점프를 하게 되면, DuringJumpng Coroutine이 발생되고, 0.8초동안 딜레이를 생성한다. (점프하는 시간)
        animator.SetBool("IsJumping", false);                               // Jump 애니메이션 동작을 마친 뒤, IsJumping 을 False형태로 두고, 땅에 착지를 했기 때문에
        isGrounded = true;                                                  // isGrounded 변수를 True로 설정한다.
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            var monsterBehaviour = collision.gameObject.GetComponent<MonsterBehaviour>();
            if (monsterBehaviour != null)
            {
                monsterBehaviour.OnAttackHit();
            }
        }
    }




    IEnumerator ComboAttack()
    {
        int damageAmount = 10;                                              // 처음에 damageAmount 는 10으로 설정한다.

        animator.SetInteger("Combo", comboCount + 1);                       // comboCount는 0으로 초기화했으며, Int형 "Combo"애니메이션에 할당한 수만큼 서로 다른 콤보공격 동작이 나타난다.
        damageAmount *= (comboCount + 1);                                   // 각 Combo공격마다, 데미지양이 다르기 때문에 comboCount가 증가함에 따라 달라지는 damageAmount값을 확인할 수 있다.

        MonsterPrefab monsterPrefab = GameObject.FindObjectOfType<MonsterPrefab>(); // MonsterPrefab Script에서,as가장 처음에 나타나는 Object를 GameObject로 반환한다. (몬스터 프리팹들...)

        if (monsterPrefab != null)                                          // monsterPrefab이 Scene에 존재하고 있을 때, monsterPrefab이 null이라면, NullReference Error 이 나오겠지
        {
            foreach (GameObject enemy in monsterPrefab.enemyList)           // List형식으로 저장한 enemyList(MonsterPrefab Script내에 있음)목록에 있는 각자의 enemy에 대하여 반복문을 수행한다.
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);    // 각자의 enemy의 위치와 내 캐릭터의 위치를 받아오는 변수 distance를 선언하고,

                if (distance <= 3.5f)                                                               // 3.5f 범위 내에 있을 시에,
                {
                    MonsterBehaviour enemyobject = enemy.GetComponent<MonsterBehaviour>();          // 각자의 enemy 몬스터(각 몬스터의 프리팹들)에 있는 Component MonsterBehaviour Script를 enemyobject라고 선언
                    bool isCritical = CriticalManager.instance.CheckCritical();                     // CriticalManager Script에 있는 CheckCritical() 메소드를 통해, 크리티컬의 여부를 판단하는 Boolean isCritical
                    enemyobject.TakeDamage(damageAmount, isCritical);
                }                                                                                   // damageAmount, isCritical 변수 두개를 받는 TakeDamage 메소드를 통해 3.5f이내 반경의 몬스터들에 대한 데미지 구현
            }                                                                                       // 동작을 실행한다. 이는 각 몬스터들이 화면 내에 존재하고 있을 때를 가정한다.
        }

        if (comboCount < 3)
        {
            foreach (var slot in skillSlots)
            {
                slot.UpdateSkillIcon(comboCount);
            }

            comboCount++;
        }
        else
        {
            comboCount = 0;
            foreach (var slot in skillSlots)
            {
                slot.UpdateSkillIcon(comboCount);
            }
        }

        float timer = 0f;

        while (timer < 0.5f)                                                                        // 0f로 초기화 된 timer 변수가 0.5f 일시, 무한반복 한다.
        {
            timer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.A))                                                        // 마우스 클릭 시 공격하는 구조기 때문에, 0.5초 이내 마우스 이벤트가 들어오면,
            {
                timer = 0f;                                                                         // timer은 다시 0f로 초기화하고 comboCount가 증가한다. (위에)
            }

            yield return null;                                                                      // yield return null 코드를 통해, while문에 내용들은 다음 프레임에 호출된다.
        }                                                                                           // 마우스 클릭 시에, timer 초기화

        animator.SetInteger("Combo", 0);                                                            // 0.5초 이내 마우스 입력이 없을 시에, Combo 애니메이션은 0으로 초기화를 하며,
        foreach (var slot in skillSlots)
        {
            slot.skillIcon.sprite = slot.defaultSprite;
        }
        comboCount = 0;                                                                             // 마찬가지로 comboCount도 0으로 초기화를 한다. (콤보애니메이션에 따라, DamageAmount 값이 동일해야 한다)
    }

    public void ResetCharacterPosition()
    {
        gameObject.transform.position = initialPosition;
        animator.SetBool("Restore", true);
    }

    public void LoadPosition(Vector3 loadedPosition)
    {
        lastposition = loadedPosition;
        transform.position = loadedPosition; // 플레이어의 위치를 변경합니다.
    }
}