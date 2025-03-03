using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera Cam;

    public Transform Character; // 카메라가 따라갈 대상 오브젝트의 Transform 변수
    public Vector3 offset = new Vector3(0f, 1f, -1f); // 카메라와 대상 오브젝트 간의 상대 위치
    public float smoothSpeed = 0.125f; // 카메라 이동 시의 부드러운 감속 정도
    public bool lookAtTarget = true; // 대상 오브젝트를 바라볼지 여부

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        Vector3 CameraPos = GetComponent<Camera>().transform.position;
        transform.position = new Vector3(CameraPos.x, CameraPos.y, CameraPos.z);


        // 대상 오브젝트를 바라보도록 카메라 회전
        if (lookAtTarget)
        {
            transform.LookAt(Character);
        }
    }
}