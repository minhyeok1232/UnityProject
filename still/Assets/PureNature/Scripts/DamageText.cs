using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageText : MonoBehaviour                 
{
    // instance
    public float destroyTime;                           // 텍스트가 사라지는 시간
    public int damage;                                 // 데미지 출력 양

    // Start
    void Start()
    {
        TextMesh textMesh = GetComponent<TextMesh>();   // 해당 오브젝트에 TestMesh 컴퍼넌트 추가 
        textMesh.text = damage.ToString();              // testMesh에 부착되어있는 text는 damage를 String형식으로 지정한다.
        Invoke("DestroyObject", destroyTime);           // 설정한 destroyTime 만큼 기다리며, 지나게되면 DestroyObject 메소드로 간다.
    }

    // Update
    void Update()
    { 
        transform.rotation = Quaternion.identity;       // 오브젝트의 회전값은 받지 않는다.
    }

    // Method
    void DestroyObject()
    {
        Destroy(gameObject);                            // 현재의 게임오브젝트를 파괴한다.
    }

}