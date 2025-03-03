using UnityEngine;

public class ClockAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    // 애니메이션을 시작하는 함수
    public void StartClockAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }
}