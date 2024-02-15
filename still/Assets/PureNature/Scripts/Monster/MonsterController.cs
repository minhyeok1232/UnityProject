using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float speed = 5f;                        // 곰이 움직이는 속도는 5.0f
    private Animator animator;                      // 곰에 있는 Animator 컴퍼넌트는 animator로 지정
    private Rigidbody rb;                           // 곰에 있는 Rigidbody 컴퍼넌트는 rb로 지정
    public GameObject player;                       // public 전역변수 - 곰이 캐릭터를 식별하고 추적하기 위한 변수 player
    private bool isAttacking = false;               // 처음에는 곰이 Attacking 동작을 하지 않게 bool변수로 false
    public int hp = 500;                            // 곰의 hp는 500으로 지정
    public int maxhp = 500;

    public PlayerController playercontroller;       // 이 스크립트는 PlayerController 스크립트를 참조한다. playercontroller로 지정

    public GameObject damageTextPrefab;
    // 이 문제인가?
    public Transform hudPos;                        // 전역변수 곰의 자식에 하나의 오브젝트를 두고 y축으로 3증가시킨 hudPos값. 이는 곰오브젝트의 자식 오브젝트를 드래그

    private Coroutine movingCoroutine;


    private enum MonsterState                          // 곰은 Idle, Moving, Attacking, Dead 4가지의 동작을 할 수 있다. (열거형으로 배열)
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    private MonsterState currentState = MonsterState.Idle;    // 

    private void Start()
    {
        animator = GetComponent<Animator>();            // animator 컴퍼넌트와
        rb = GetComponent<Rigidbody>();                 // rigidbody 컴퍼넌트를 실행시키기 위한 Start 구문

        // 타스크립트에있는 Character 참조하기


        GameObject playerObject = GameObject.FindGameObjectWithTag("Cha"); // "Cha"라는 태그를 가진 오브젝트를 찾습니다.
        playercontroller = playerObject.GetComponent<PlayerController>();  // 찾은 오브젝트에서 PlayerController 컴포넌트를 가져옵니다.

        StartCoroutine(StateMachine());
    }

    public void TakeDamage(int damageAmount, bool isCritical)
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        DamageText damageText = damageTextObject.GetComponentInChildren<DamageText>();
        damageTextObject.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        damageText.transform.position = hudPos.position+Random.Range(-2,2)*Vector3.one;
        damageText.damage = damageAmount;

        if (isCritical)
        {
            damageAmount = damageAmount * 2;
            damageText.damage = damageAmount;

            TextMesh textMesh = damageTextObject.GetComponent<TextMesh>();

            if (textMesh != null)
            {
                textMesh.color = Color.red; // 텍스트 색상을 빨간색으로 변경
                Debug.Log("크리티컬 발동, 텍스트 색상 변경");
            }
            else
            {
                Debug.Log("TextMesh 컴포넌트를 찾을 수 없습니다.");
            }
        }

        hp -= damageAmount;
        
    }



    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case MonsterState.Idle:
                    yield return StartCoroutine(IdleState());
                    break;
                case MonsterState.Moving:
                    yield return StartCoroutine(MovingState());
                    break;
                case MonsterState.Attacking:
                    yield return StartCoroutine(AttackingState());
                    break;
                case MonsterState.Dead:
                    yield return StartCoroutine(DeadState());
                    break;
            }
        }
    }

    private IEnumerator IdleState()
    {
        animator.SetBool("WalkForward", false);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        currentState = MonsterState.Moving;
        movingCoroutine = null;
    }

    private IEnumerator MovingState()
    {
        animator.SetBool("WalkForward", true);

        Vector3 moveDirection = GetDirectionToPlayer();

        float DirectionTimer = 0.0f;
        float timeToChangeDirection = 2.0f;

        while (currentState == MonsterState.Moving || currentState == MonsterState.Attacking)
        {
            if (player != null)
            {
                moveDirection = GetDirectionToPlayer();
            }

            else if (currentState == MonsterState.Moving)
            {
                DirectionTimer += Time.deltaTime;

                if (DirectionTimer > timeToChangeDirection)                 // 랜덤으로 이동하는 로직
                {
                    moveDirection = GetRandomDirection();                   // 랜덤 방향 
                    DirectionTimer = 0.0f;
                }
            }

 

            OrientAndMoveInDirection(moveDirection);

            yield return null;

            if (hp <= 0)
            {
                currentState = MonsterState.Dead;
            }
        }

        movingCoroutine = null;
    }

    private Vector3 GetDirectionToPlayer()
    {
        return (playercontroller.transform.position - transform.position).normalized;
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    private void OrientAndMoveInDirection(Vector3 moveDirection)
    {
        var targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
    }

    private IEnumerator DeadState()
    {
        if (movingCoroutine != null) 
        {
            StopCoroutine(movingCoroutine);
        }

        animator.SetBool("Death", true);

        yield return new WaitForSeconds(2f);

        animator.SetBool("WalkForward", false);

        Destroy(gameObject);

    }

    private IEnumerator AttackingState()
    {
        isAttacking = true;
        animator.SetBool("WalkForward", false);
        animator.SetBool("Attack2", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack2", false);
        isAttacking = false;

        while (currentState == MonsterState.Attacking && Vector3.Distance(transform.position, player.transform.position) > 2f)
        {
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;

            var targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);

            yield return null;
        }

        currentState = MonsterState.Idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cha"))
        {
            if (currentState != MonsterState.Attacking && !isAttacking)
            { 
                player = collision.gameObject; // 캐릭터를 추적 대상으로 설정

                rb.velocity = Vector3.zero; // 여기에 추가합니다.

                isAttacking = true;
                animator.SetBool("WalkForward", false);
                animator.SetBool("Attack1", true);
                currentState = MonsterState.Attacking;
                StartCoroutine(AttackCooldown());
            }
        }
    }



        private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1f);

        if (currentState == MonsterState.Attacking)
        {
            isAttacking = false;
            animator.SetBool("Attack1", false);
            animator.SetBool("WalkForward", true);
            currentState = MonsterState.Moving;
        }
    }
}
