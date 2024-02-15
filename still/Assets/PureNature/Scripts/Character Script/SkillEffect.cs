using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour            // Character에 들어간 Script
{
    public ParticleSystem[] swordEffects;           // ParticleSystem Component를 배열로 담아서,

    public void PlaySwordEffect(int index)          // int형 index 변수마다 각자 다른 이펙트들을 할당하고,
    { 
        swordEffects[index].Play();                 // 이 이펙트들을 Play() 할 수 있는 메소드를 적용시켜준다.
    }
}