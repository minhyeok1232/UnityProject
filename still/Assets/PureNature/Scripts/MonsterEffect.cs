using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEffect : MonoBehaviour            // Character�� �� Script
{
    public ParticleSystem swordEffect;          // ParticleSystem Component�� �迭�� ��Ƽ�,

    public void PlaySwordEffect()          // int�� index �������� ���� �ٸ� ����Ʈ���� �Ҵ��ϰ�,
    {
        swordEffect.Play();                 // �� ����Ʈ���� Play() �� �� �ִ� �޼ҵ带 ��������ش�.
    }
}