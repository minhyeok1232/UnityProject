using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MovableHeaderUI : MonoBehaviour, IDragHandler     // UnityEngine.EventSystems �������̽��� �����Ǵ� IPointerDownHandler, IDragHandler �������̽� ����
{
    // Transform
    [SerializeField]
    private Transform targetTr;                                                     // ����ȭ ������ ���� private�� ������ Transform �� _targetTr(�̵� �� UI) ����

    private Vector2 originalPosition;                                               // ó�� ��ġ�� ������ ������ �߰�

    // Vector
    private Vector2 beginPoint;                                                     // 3D ����, UI�� 2D�� ����. UI�� ��ġ�� �����ϴ� ������ ����
    private Vector2 moveBegin;                                                      // ���콺 ��ġ�� �����ϴ� ������ ����

    // Awake
    private void Awake()
    {
        if (targetTr == null)                                                       // Awake ������ targetTr�� �������� ���� ��, 
            targetTr = transform;                                                   // �� �θ�(Inventory)�� ��ġ�� �ʱ�ȭ�� �����ش�.
                                                                                    // �̴� ���� ���� �� �ٷ� ������ �ȴ�.
        originalPosition = targetTr.localPosition;                                  // Awake���� ó�� ��ġ ����

        beginPoint = originalPosition;
    }



    void IDragHandler.OnDrag(PointerEventData eventData)                            // IDragHandler �������̽��� ����Ǿ��ִ� OnDrag�޼ҵ�� ����������
    {
        Vector3 temp = Camera.main.ScreenToViewportPoint(eventData.delta);
        temp.x *= 1080;
        temp.y *= 1080;
        targetTr.localPosition = targetTr.localPosition + temp;                     // ���⼭�� eventData.position  - �巡���ϴ� ���콺�� ���� ��ġ�� �޾Ƽ� ������ moveBegin�� ���콺��ġ ��ǥ�� ����
    }                                                                               // ������Ʈ�� UI�� ��ġ�� �޾Ƽ� ������Ʈ���ش�.
}