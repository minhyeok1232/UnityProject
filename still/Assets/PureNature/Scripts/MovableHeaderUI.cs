using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MovableHeaderUI : MonoBehaviour, IDragHandler     // UnityEngine.EventSystems 인터페이스에 지원되는 IPointerDownHandler, IDragHandler 인터페이스 선언
{
    // Transform
    [SerializeField]
    private Transform targetTr;                                                     // 직렬화 과정을 통해 private로 선언한 Transform 형 _targetTr(이동 될 UI) 선언

    private Vector2 originalPosition;                                               // 처음 위치를 저장할 변수를 추가

    // Vector
    private Vector2 beginPoint;                                                     // 3D 지만, UI는 2D로 구성. UI의 위치를 저장하는 벡터형 변수
    private Vector2 moveBegin;                                                      // 마우스 위치를 저장하는 벡터형 변수

    // Awake
    private void Awake()
    {
        if (targetTr == null)                                                       // Awake 문에서 targetTr이 지정되지 않을 시, 
            targetTr = transform;                                                   // 그 부모(Inventory)의 위치로 초기화를 시켜준다.
                                                                                    // 이는 게임 시작 후 바로 실행이 된다.
        originalPosition = targetTr.localPosition;                                  // Awake에서 처음 위치 저장

        beginPoint = originalPosition;
    }



    void IDragHandler.OnDrag(PointerEventData eventData)                            // IDragHandler 인터페이스에 내장되어있는 OnDrag메소드는 마찬가지로
    {
        Vector3 temp = Camera.main.ScreenToViewportPoint(eventData.delta);
        temp.x *= 1080;
        temp.y *= 1080;
        targetTr.localPosition = targetTr.localPosition + temp;                     // 여기서의 eventData.position  - 드래그하는 마우스의 현재 위치를 받아서 이전의 moveBegin의 마우스위치 좌표를 통해
    }                                                                               // 업데이트된 UI의 위치를 받아서 업데이트해준다.
}