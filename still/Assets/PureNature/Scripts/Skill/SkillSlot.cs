using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillSlot
{
    public Image skillIcon; // Unity Inspector���� �Ҵ�
    public Sprite defaultSprite; // �⺻ ��������Ʈ (�޺��� �ƴ� ��)
    public Sprite[] comboSprites; // �޺� ��������Ʈ �迭 (�޺� ������ �� �� �ܰ迡 �ش��ϴ� ��������Ʈ)
    public int comboActivationNumber; // �� ��ų ������ Ȱ��ȭ�Ǵ� �޺� ��

    // ��ų ������ ������Ʈ�ϴ� �޼���
    public void UpdateSkillIcon(int comboCount)
    {
        if (comboCount >= 0 && comboCount <= comboSprites.Length)
        {
            skillIcon.sprite = comboSprites[comboCount];
        }
    }
}