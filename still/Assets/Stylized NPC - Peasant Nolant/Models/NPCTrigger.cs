using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCTrigger : MonoBehaviour
{
    //public ObjData obj;       // 합칠 예정

    // Start
    void Start()
    {
        //mainScript = GameObject.Find("Main").GetComponent<MainScript>();    // 두 오브젝트의 상호작용은 MainScript에서 관리를 한다.
    }

    // Trigger Event
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cha")
        {
            //mainScript.StartChat(this);                             // NPC에게들어가는 스크립트로서, 캐릭터랑 충돌하게 된다면
            var inter = transform.parent.GetComponent<Iinterface>();
            if (inter != null)
            {
                inter.enter();
            }
        }                                                           // mainScript에 StartChat메소드를 참조한다.
    }                                                               // (this)는 StartChat메소드가 NpcTrigger형 변수 npc를 받게 된다.

    public void OnTriggerExit(Collider other)                      // 이후 Exit을 하게 되면,
    {
        if (other.tag == "Cha")
        {
            //mainScript.EndChat(this);                               // 그 상태에서 대화 상호작용은 끝나게 되며,
            //mainScript.chatIndex = 0;                               // 처음부터 다시 대화를 하는 순서기능을 0으로 다시 만들어준다.
            var inter = transform.parent.GetComponent<Iinterface>();
            if (inter != null)
            {
                inter.exit();
            }
        }
    }
}