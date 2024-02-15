using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera Cam;

    public Transform Character; // ī�޶� ���� ��� ������Ʈ�� Transform ����
    public Vector3 offset = new Vector3(0f, 1f, -1f); // ī�޶�� ��� ������Ʈ ���� ��� ��ġ
    public float smoothSpeed = 0.125f; // ī�޶� �̵� ���� �ε巯�� ���� ����
    public bool lookAtTarget = true; // ��� ������Ʈ�� �ٶ��� ����

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        Vector3 CameraPos = GetComponent<Camera>().transform.position;
        transform.position = new Vector3(CameraPos.x, CameraPos.y, CameraPos.z);


        // ��� ������Ʈ�� �ٶ󺸵��� ī�޶� ȸ��
        if (lookAtTarget)
        {
            transform.LookAt(Character);
        }
    }
}