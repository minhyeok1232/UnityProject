using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillSlot
{
    public Image skillIcon; // Unity Inspector에서 할당
    public Sprite defaultSprite; // 기본 스프라이트 (콤보가 아닐 때)
    public Sprite[] comboSprites; // 콤보 스프라이트 배열 (콤보 상태일 때 각 단계에 해당하는 스프라이트)
    public int comboActivationNumber; // 이 스킬 슬롯이 활성화되는 콤보 수

    // 스킬 슬롯을 업데이트하는 메서드
    public void UpdateSkillIcon(int comboCount)
    {
        if (comboCount >= 0 && comboCount <= comboSprites.Length)
        {
            skillIcon.sprite = comboSprites[comboCount];
        }
    }
}