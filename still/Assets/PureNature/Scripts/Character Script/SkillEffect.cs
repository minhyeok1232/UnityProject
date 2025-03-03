using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour            // Character�� �� Script
{
    public ParticleSystem[] swordEffects;           // ParticleSystem Component�� �迭�� ��Ƽ�,

    public void PlaySwordEffect(int index)          // int�� index �������� ���� �ٸ� ����Ʈ���� �Ҵ��ϰ�,
    { 
        swordEffects[index].Play();                 // �� ����Ʈ���� Play() �� �� �ִ� �޼ҵ带 ��������ش�.
    }
}