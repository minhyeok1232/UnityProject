using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DropItem
{
    public GameObject item;                         // 드롭될 아이템
    public float dropChance;                        // 아이템이 드롭될 확률 (0 ~ 1 사이의 값으로, 예: 0.5는 50%)
}

public class MonsterBehaviour : MonoBehaviour
{
    // Script
    public MonsterInformation monsterInformation;   // 몬스터들이 가지고있는 Hp, Exp 등등을 관리한다.
    public PlayerController playercontroller;       // 이 스크립트는 PlayerController 스크립트를 참조한다. playercontroller로 지정
    public DamageAnimHelper damageAnimHelper;
    public MainScript main;
    public int Hp { get; set; }                     // MonsterBehaviour 클래스의 Hp 속성

    // Component
    private Animator animator;                      // 애니메이션    
    private Rigidbody rb;                           // 중력

    // GameObject
    public GameObject player;                       // 몬스터가 캐릭터를 식별하고 추적하기 위한 변수 player
    public GameObject damageTextPrefab;             // 데미지 Text형식의 Prefab 
    public GameObject MonsterGetDamagePrefab;       // 몬스터가 주는 데미지 Prefab

    // Instance
    public float speed = 5.0f;                      // 곰이 움직이는 속도는 5.0f
    private float timer = 0.0f;
    private float timer2 = 0.0f;
    private float lastCollisionTime; // 마지막으로 OnCollisionEnter가 호출된 시간
    public float damageInterval = 0.2f;
    // Transform
    public Transform hudPos;                        // 몬스터 자식에 설정한 hudPos위치에 데미지출력학히 위함
    public Transform MonsterColliderPrefab;

    // Boolean
    private bool isColliderOn = false;               // 처음에는 몬스터가 공격동작을 안함. (공격동작 O, X)
    private bool isAttacking = false;
    public bool MonstersCollider = true; 

    // Coroutine
    public Coroutine movingCoroutine;              // Coroutine 변경 참조 변수

    // List
    public List<DropItem> dropItems; // 인스펙터에서 아이템과 확률을 설정할 수 있는 리스트

    // Enum 
    private enum MonsterState                       // 몬스터의 4가지 상태
    {
        Idle,
        Moving,
        //Attacking,
        Dead
    }

    private MonsterState currentState = MonsterState.Idle;                  // Enum으로 선언한 MonsterState 클래스의 초기값은 Idle상태임을 나타낸다.

    // Unity 시작 될 때 처음에 초기화 되는 값
    void Start()                                                
    {
        monsterInformation = GetComponent<MonsterInformation>();            // monsterInformation 은 MonsterInformation Script를 컴퍼넌트로 가지고 있다.
        animator = GetComponent<Animator>();                                // Prefab화된 몬스터 Object들은 Animator, Rigidbody 컴퍼넌트를 가지고 있다.
        rb = GetComponent<Rigidbody>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Cha");  // "Cha"라는 태그를 가진 오브젝트를 찾는다.
        playercontroller = playerObject.GetComponent<PlayerController>();   // 찾은 오브젝트에서 PlayerController 컴포넌트를 가져온다.

        StartCoroutine(StateMachine());                                     // StateMachine의 Coroutine 시작
    }
    // Update
    private void Update()
    {
        if(isColliderOn)
        {
            timer += Time.deltaTime;
            
            if(timer > 1.0f)
            {
                OnColliderHit();
                timer = 0.0f;
            }
        }

        TextMesh textMesh = damageAnimHelper.GetComponent<TextMesh>();

        if (textMesh == null)
        {
            textMesh = GetComponent<TextMesh>();
        }

        if (currentState == MonsterState.Idle)
            return;
        
    }


    // Coroutine
    private IEnumerator StateMachine()                                      // StateMAchine Coroutine
    {
        while (true)                                                        // 각 State에서 yield 구문을 쓰기 때문에, 프레임 드롭이 나타나지 않는다.
        {
            switch (currentState)                                           // Swtich-case 구문으로 각 몬스터들의 상태를 나타낸다.
            {
                case MonsterState.Idle:
                    yield return StartCoroutine(IdleState());
                    break;
                case MonsterState.Moving:
                    yield return StartCoroutine(MovingState());
                    break;
                /*case MonsterState.Attacking:
                    yield return StartCoroutine(AttackingState());*/
                    break;
                case MonsterState.Dead:
                    yield return StartCoroutine(DeadState());
                    break;
            }
        }
    }

    private IEnumerator IdleState()                             // 가만히 있는 상태에 대한 동작
    {
        animator.SetBool("WalkForward", false);                 // Bool 형태의 WalkForward는 False
        yield return new WaitForSeconds(Random.Range(1f, 3f));  // 1초 ~ 3초사이에 무작위 시간을 지연시킨다.
        currentState = MonsterState.Moving;                     // 이 후, 몬스터의 상태를 Moving 상태로 변경한다.

        movingCoroutine = null;                                 // 다음 상태 시작 준비를 위해 null로 초기화 한다.
    }

    private IEnumerator MovingState()                           // 움직이는 상태에 대한 동작 
    {
        animator.SetBool("WalkForward", true);                  // Bool 형태의 WalkForward는 True로 활성화

        Vector3 moveDirection = GetRandomDirection();           // 평상시에는 랜덤으로 이동하기 위한 로직의 좌표값을 메소드로 받아준다.

        float DirectionTimer = 0.0f;                            // Time.deltaTime 으로
        float timeToChangeDirection = 2.0f;                     // 2초마다 방향전환을 위한 시간변수(float)

        while (currentState == MonsterState.Moving)   // 적오브젝트가 이동중이거나, 공격중일 때
        {
            if (player != null)                                 // player (추적할 대상이 인식된 상태라면)
            {
                moveDirection = GetDirectionToPlayer();         // 캐릭터를 추적하기 위해 메소드로 반환된 좌표값을 받아주고
            }

            else 
            {                                                   // 추적할 대상이 아직 인식되지 않았다면,
                DirectionTimer += Time.deltaTime;               // 타이머를 재서

                if (DirectionTimer > timeToChangeDirection)     // 2초를 넘긴다면,
                {
                    moveDirection = GetRandomDirection();       // 다시 랜덤한 방향으로 이동하게 되며,
                    DirectionTimer = 0.0f;                      // DirectionTimer 은 0초로 다시 초기화를 해준다.
                }
            }

            OrientAndMoveInDirection(moveDirection);            // moveDirection 좌표값을 받아서 부드럽게 회전하기 위한 옵션을 넣어준다. (Method)

            yield return null;
        }
    }


    public IEnumerator DeadState()                              // 곰이 죽을 때 동작
    {
        animator.SetBool("WalkForward", false);
        animator.SetBool("Death", true);                         // Death 애니메이션을 True로 해주고,

        DropLoot();

        yield return new WaitForSeconds(2f);                     // 2초를 기다리고

        if ( this != null)
            Destroy(gameObject);
    }

    public void TakeDamage(int damageAmount, bool isCritical)   // 여기안에서 
    {
        var prefabPooling = ObjectPooler.GetObject();
        Transform damagePos1 = this.transform.Find("DamagePos1");
        if (damagePos1 != null)
        {
            prefabPooling.transform.SetParent(damagePos1);
            prefabPooling.transform.localPosition = Vector3.one * Random.Range(-10, 11) / 10.0f;
        }

        DamageAnimHelper damageAnimHelper = prefabPooling.GetComponent<DamageAnimHelper>();
        TextMesh textMesh = damageAnimHelper.GetComponent<TextMesh>();
        textMesh.color = new Color(1, 0.2f, 0);  // 오렌지 (빨간색에 가까운)
        textMesh.fontSize = 18;
        if (isCritical)
        {
            float multiplier = Random.Range(1.6f, 2.15f);
            damageAmount *= (int)multiplier;
            if (textMesh != null)
            {
                textMesh.color = Color.red;
                textMesh.fontSize = 30;
            }
        }

        damageAnimHelper.Damage = damageAmount;

        Hp -= damageAmount;

        if (Hp <= 0)
        {
            StopAllCoroutines();
        }
    }


    private Vector3 GetDirectionToPlayer()                                  
    {
        return (playercontroller.transform.position - transform.position).normalized;                   // 벡터는 플레이어를 가리키는 방향과 거리를 정규화
    }                                                                                                   // 몬스터 오브젝트가 플레이어를 향한 방향

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;                // 랜덤 방향 (처음에 플레이어를 인식하기 전, 랜덤하게 이동)
    }

    private void OrientAndMoveInDirection(Vector3 moveDirection)
    {
        var targetRotation = Quaternion.LookRotation(moveDirection);                                    // 주어진 moveDirection 벡터를 바라보는 회전을 나타내는 쿼터니언 계산
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 현재 바라보는 방향에서 targetRotation 으로, Time.deltaTime*5의 속도로
                                                                                                        // 회전을 진행한다. (Quaternion.Slerp : 부드럽게 회전시켜주는 옵션)
        rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);                          // 현재 몬스터가 moveDirection 방향으로 이동. 주어진 speed만큼 이동을 하며,
    }                                                                                                   // Time.deltaTime으로 프레임속도에 따른 변동을 보정한다.
                                                                                                        // rb.position 은 해당 객체의 현재 위치를 나타낸다.

    private void Chasing(GameObject player)                                                             // player 변수를 받아, 추적하는 메소드
    {
        if (player == null) return;
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;              // 플레이어의 위치와 해당 컴퍼넌트가 들어간 오브젝트(적)의 위치를 정규화를 하고,

        var targetRotation = Quaternion.LookRotation(moveDirection);                                      // moveDirection의 위치로 방향을 바꿔주는 쿼터니언값을 targetRotation으로 지정하며,
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);   // 곰오브젝트의 방향으로부터, 캐릭터를 바라보는 방향으로, 해당 스피드만큼 부드럽게 회전해주는 
                                                                                                          // 옵션을 추가하였다.
        rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);

        MonstersAttackingFalse();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == MonsterState.Idle)
        {
            return;
        }


        // 현재 시간이 마지막 충돌 시간 + 쿨다운 시간보다 작다면 (즉, 쿨다운 시간 안에 있다면) 충돌을 무시
        if (Time.time < lastCollisionTime + 1.0f)
            return;


        if (collision.gameObject.CompareTag("Cha") && !isColliderOn)                                                      // Cha(Tag로 설정된 나의 캐릭터)인 Tag들에 닿게 된다면,
        {
            OnColliderHit();
            timer = 0.0f;
            isColliderOn = true;


            if (isAttacking)
            {
                return;
            }

            isAttacking = true;

            if(isColliderOn)
            {
                animator.SetBool("WalkForward", true);                  // Attacking 애니메이션을 펼친 후 캐릭터를 추적해야 하기 때문에, WalkFoward는 true
                animator.SetTrigger("Attack1");                         // 공격하는 동작은 당연히 True
            }

            isAttacking = false;
            Chasing(player);


            player = collision.gameObject;                                                              // 캐릭터를 추적 대상으로 설정
            Chasing(player);                                                                            // Chasing Method 실행

            lastCollisionTime = Time.time; // 마지막 충돌 시간을 현재 시간으로 업데이트
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cha"))
        {
            isColliderOn = false;
        }
    }


    void DropLoot()
    {
        foreach (DropItem dropItem in dropItems)
        {
            float randomValue = Random.Range(0f, 1f);
            if (randomValue <= dropItem.dropChance)
            {
                GameObject droppedItem = Instantiate(dropItem.item, this.transform.position, Quaternion.identity);
                Rigidbody rb = droppedItem.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 추가
                rb.useGravity = true; // 중력 활성화
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // 약간의 위쪽으로 힘을 가함
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("atk_z"))
        {
            // 지속 데미지 코루틴 시작
            StartCoroutine(ApplyDamageOverTime(other, 2.0f, damageInterval));
        }
        if (other.CompareTag("atk_v"))
        {
            ApplyDamage2(other);
        }
        if (other.CompareTag("atk_d"))
        {
            StartCoroutine(ApplyDamageOverTime2(other, 3.0f, 0.15f));
        }
        if (other.CompareTag("atk_x"))
        {
            StartCoroutine(ApplyDamageOverTime3(other, 3.0f, 0.33f));
        }
        if (other.CompareTag("atk_c"))
        {
            StartCoroutine(ApplyDamageOverTime4(other, 3.0f, 0.2f));
        }
    }
    // 지속적으로 데미지를 주는 코루틴
    IEnumerator ApplyDamageOverTime(Collider target, float duration, float interval)
    {
        float timer = 0;

        while (timer < duration)
        {
            // 데미지를 주는 로직을 여기에 구현
            ApplyDamage(target);

            // 다음 데미지 전까지 대기
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
    IEnumerator ApplyDamageOverTime2(Collider target, float duration, float interval)
    {
        float timer = 0;

        while (timer < duration)
        {
            // 데미지를 주는 로직을 여기에 구현
            ApplyDamage3(target);

            // 다음 데미지 전까지 대기
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
    IEnumerator ApplyDamageOverTime3(Collider target, float duration, float interval)
    {
        float timer = 0;

        while (timer < duration)
        {
            // 데미지를 주는 로직을 여기에 구현
            ApplyDamage4(target);

            // 다음 데미지 전까지 대기
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
    IEnumerator ApplyDamageOverTime4(Collider target, float duration, float interval)
    {
        float timer = 0;

        while (timer < duration)
        {
            // 데미지를 주는 로직을 여기에 구현
            ApplyDamage5(target);

            // 다음 데미지 전까지 대기
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
    IEnumerator ApplyDamageOverTime5(Collider target, float duration, float interval)
    {
        float timer = 0;

        while (timer < duration)
        {
            // 데미지를 주는 로직을 여기에 구현
            ApplyDamage6(target);

            // 다음 데미지 전까지 대기
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
    IEnumerator ApplyDamageOverTime6(Collider target, float duration, float interval)
    {
        float timer = 0;

        while (timer < duration)
        {
            // 데미지를 주는 로직을 여기에 구현
            ApplyDamage7(target);

            // 다음 데미지 전까지 대기
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }

    // 실제 데미지를 주는 메서드
    private void ApplyDamage(Collider monster)
    {
        float multiplier = Random.Range(1.1f, 1.5f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);
    }
    private void ApplyDamage2(Collider monster)
    {
        float multiplier = Random.Range(26.4f, 29.7f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);
    }
    private void ApplyDamage3(Collider monster)
    {
        float multiplier = Random.Range(2.4f, 2.8f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);
    }
    private void ApplyDamage4(Collider monster)
    {
        float multiplier = Random.Range(5.9f, 7.7f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);
    }
    private void ApplyDamage5(Collider monster)
    {
        float multiplier = Random.Range(3.2f, 3.4f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);

        StartCoroutine(ApplyDamageOverTime5(monster, 2.0f, 1.0f));
    }
    private void ApplyDamage6(Collider monster)
    {
        float multiplier = Random.Range(8.2f, 10.2f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);

        StartCoroutine(ApplyDamageOverTime6(monster, 2.0f, 0.15f));
    }
    private void ApplyDamage7(Collider monster)
    {
        float multiplier = Random.Range(2.9f, 4.4f);
        int damage = Mathf.RoundToInt(monsterInformation.monster.Damage * multiplier);
        bool isCritical = CriticalManager.instance.CheckCritical();
        TakeDamage(damage, isCritical);
    }

    public void OnAttackHit()
    {
        if (isColliderOn)
        {
            var prefabPooling = ObjectPooler.GetObject();
            Transform CharacterPos = player.transform.Find("CharacterPos");
            if (CharacterPos != null)
            {
                prefabPooling.transform.SetParent(CharacterPos);
                prefabPooling.transform.localPosition = Vector3.zero;
            }
            TestSlider testSlider = GameObject.FindObjectOfType<TestSlider>();
            MonsterInformation monsterInfo = GetComponent<MonsterInformation>();

            DamageAnimHelper damageAnimHelper = prefabPooling.GetComponent<DamageAnimHelper>();

            float multiplier = Random.Range(1.5f, 1.8f);
            damageAnimHelper.Damage = Mathf.RoundToInt(monsterInfo.monster.Damage * multiplier);

            TextMesh textMesh = damageAnimHelper.GetComponent<TextMesh>();

            if (textMesh != null)
            {
                textMesh.color = new Color(0.25f, 0f, 0.25f);
                textMesh.fontSize = 14;
            }

            testSlider.PlayerHealth -= damageAnimHelper.Damage;
        }
    }

    public void OnColliderHit()
    {
        var prefabPooling = ObjectPooler.GetObject();
        Transform CharacterPos = GameObject.Find("Character").transform;
        if (CharacterPos != null)
        {
            prefabPooling.transform.SetParent(CharacterPos);
            prefabPooling.transform.localPosition = Vector3.zero;
        }
        TestSlider testSlider = GameObject.FindObjectOfType<TestSlider>();
        MonsterInformation monsterInfo = GetComponent<MonsterInformation>();

        DamageAnimHelper damageAnimHelper = prefabPooling.GetComponent<DamageAnimHelper>();
        damageAnimHelper.Damage = monsterInfo.monster.Damage;


        TextMesh textMesh = damageAnimHelper.GetComponent<TextMesh>();

        if (textMesh != null)
        {
            textMesh.color = new Color(0.25f, 0f, 0.25f);
            textMesh.fontSize = 14;
        }

        testSlider.PlayerHealth -= damageAnimHelper.Damage;
    }
    void MonstersAttackingFalse()
    {
        MonstersCollider = false;
        UpdateCollisionState(false);
    }

    void UpdateCollisionState(bool isColliding)
    {
        int layer1 = LayerMask.NameToLayer("Enemy");

        if (isColliding)
        {
            Physics.IgnoreLayerCollision(layer1, layer1, true);           // 몬스터기리 닿아있어서 충돌을 해제 해준다.
        }
        else
        {
            Physics.IgnoreLayerCollision(layer1, layer1, false);          // 몬스터끼리 닿아있어서, 충돌을 체크해준다.
        }
    }
    public void CharacterDieFunction()
    {
        StartCoroutine(MovingState());
    }

    public void DisableMonsterAttacking()
    {
        currentState = MonsterState.Moving;
        isColliderOn = false;

        player = null;
    }
}