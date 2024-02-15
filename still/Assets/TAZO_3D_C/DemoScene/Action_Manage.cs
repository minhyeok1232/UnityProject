using UnityEngine;
using System.Collections;
[System.Serializable]
public struct AttackState
{
	public System.Action attackAction; // 해당 공격의 함수를 실행하기 위한 델리게이트
	public float probability; // 해당 공격의 확률
}


public class Action_Manage : MonoBehaviour {
	public GameObject Target;
	Animator myAnimator;

	public AttackState[] Attacking; // 인스펙터에서 아이템과 확률을 설정할 수 있는 배열
									// Use this for initialization
	void Start () {
		myAnimator = Target.GetComponent<Animator> ();

		// 공격 배열 초기화
		Attacking = new AttackState[]
		{
		new AttackState { attackAction = Pressed_attack_01, probability = 0.4f },
		new AttackState { attackAction = Pressed_attack_02, probability = 0.4f },
		new AttackState { attackAction = Pressed_attack_03, probability = 0.2f }
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void ClearAllBool(){
		myAnimator.SetBool ("defy", false);
		myAnimator.SetBool ("idle",  false);
		myAnimator.SetBool ("dizzy", false);
		myAnimator.SetBool ("walk", false);
		myAnimator.SetBool ("run", false);
		myAnimator.SetBool ("jump", false);
		myAnimator.SetBool ("die", false);
		myAnimator.SetBool ("jump_left", false);
		myAnimator.SetBool ("jump_right", false);
		myAnimator.SetBool ("attack_01", false);
		myAnimator.SetBool ("attack_03", false);
		myAnimator.SetBool ("attack_02", false);
		myAnimator.SetBool ("damage", false);
	}
	public void Pressed_damage(){
		ClearAllBool();
		myAnimator.SetBool ("damage", true);
	}
	public void Pressed_idle(){
		ClearAllBool();
		myAnimator.SetBool ("idle", true);
	}
	public void Pressed_defy(){
		ClearAllBool();
		myAnimator.SetBool ("defy", true);
	}
	public void Pressed_dizzy(){
		ClearAllBool();
		myAnimator.SetBool ("dizzy", true);
	}
	public void Pressed_run(){
		ClearAllBool();
		myAnimator.SetBool ("run", true);
	}
	public void Pressed_walk(){
		ClearAllBool();
		myAnimator.SetBool ("walk", true);
	}
	public void Pressed_die(){
		ClearAllBool();
		myAnimator.SetBool ("die", true);
	}
	public void Pressed_jump(){
		ClearAllBool();
		myAnimator.SetBool ("jump", true);
	}
	public void Pressed_jump_left(){
		ClearAllBool();
		myAnimator.SetBool ("jump_left", true);
	}
	public void Pressed_jump_right(){
		ClearAllBool();
		myAnimator.SetBool ("jump_right", true);
	}

	// 공격 랜덤 배열
	public void Pressed_attack_01(){
		ClearAllBool();
		myAnimator.SetBool ("attack_01", true);
	}
	public void Pressed_attack_02(){
		ClearAllBool();
		myAnimator.SetBool ("attack_02", true);
	}
	public void Pressed_attack_03(){
		ClearAllBool();
		myAnimator.SetBool ("attack_03", true);
	}
}
