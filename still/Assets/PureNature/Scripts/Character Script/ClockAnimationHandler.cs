using UnityEngine;

public class ClockAnimationHandler : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    // �ִϸ��̼��� �����ϴ� �Լ�
    public void StartClockAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
    }
}