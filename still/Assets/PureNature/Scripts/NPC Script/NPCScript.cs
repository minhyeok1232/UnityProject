using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour, Iinterface
{
    private Animator animator;                  // NPC에랑 닿게된다면, Animator을 넣어야함

    // Script
    public MainScript mainScript;              // MainScript를 참조 받는 변수
    public QuestInfo questInfo;
    public PlayerController player;

    // GameObject
    public GameObject NPCDialog;                // NPCDialog
    public GameObject PlayerDialog;             // PlayerDialog
    public GameObject EXPBar;
    public GameObject Skill_UI;

    // Text
    public Text NPCTextUI;
    public Text PlayerTextUI;

    // String 
    public string ChatText = null;              // 처음의 ChatText 은 초기화 시켜준다.

    // instance
    public int relatedQuestID;                  // 이 NPC와 연관된 퀘스트의 ID

    private void Start()
    {
        animator = GetComponent<Animator>();    // Animator 을 발생 시키기 위한 컴퍼넌트 (NPC 오브젝트)
        mainScript = FindObjectOfType<MainScript>();
    }
    public void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Cha")
        {
            animator.SetBool("Hello", true);

            if (!ReferenceEquals(mainScript.currentAcceptedNPC, null) && !ReferenceEquals(mainScript.currentAcceptedNPC, this))
            {
                return;
            }

            Skill_UI.SetActive(false);
            mainScript.currentChattedNPC = this;

            if (mainScript.currentDialogueState == MainScript.DialogueState.Declined)
            {
                mainScript.currentDialogueState= MainScript.DialogueState.Introduction;
            }

            if (mainScript.QuestProgressing.TryGetValue(relatedQuestID, out QuestProgress progress))
            {
                if (progress.questStatus == QuestProgress.Status.Proceeding)
                {
                    return;
                }
                else if (this.gameObject == questInfo.targetNPC && progress.questStatus == QuestProgress.Status.Ending)
                {
                    mainScript.StartChat(questInfo);
                }
                else
                {
                    mainScript.CheckIsLevelOn(out List<QuestInfo> availableQuests, this);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)  // 마찬가지로 Exit하게 된다면,
    {
        animator.SetBool("Hello", false);       // Hello 애니메이션을 false해준다.
        PlayerDialog.SetActive(false);
        NPCDialog.SetActive(false);
        Skill_UI.SetActive(true);

        EXPBar.SetActive(true);
        player.canAttack = true;

        // 대화 인덱스 초기화
        mainScript.chatIndex = 0;

        // 버튼의 상태도 초기 상태로 돌려놓습니다.
        mainScript.Accept.SetActive(false);
        mainScript.Refuse.SetActive(false);
        mainScript.NextButton.gameObject.SetActive(true);
        mainScript.IsAvailable.SetActive(false);

        mainScript.Minimap.GetComponent<CanvasGroup>().alpha = 1.0f;
        mainScript.currentChattedNPC = null;
    }


    public void StartAssignedQuest()
    {
        QuestProgress progress = mainScript.QuestProgressing[relatedQuestID];

        if (questInfo != null)
        {
            relatedQuestID = questInfo.id;  // 여기서 해당 NPC와 연관된 퀘스트의 ID를 설정합니다.
            if (mainScript != null)
            {
                if (progress.questStatus == QuestProgress.Status.Ending)
                {
                    mainScript.StartChat(questInfo);
                }
                else if (progress.questStatus == QuestProgress.Status.Completed)
                {
                    mainScript.StartFirst(this, questInfo);
                    mainScript.QuestStartImage.SetActive(false);
                    mainScript.IsAvailable.SetActive(false);
                }
                else
                    mainScript.StartFirst(this, questInfo);

            }
        }
    }

    public void enter()
    {
        //mainScript.StartChat(this);
    }
    public void exit()
    {
        //mainScript.EndChat(this);
    }
}
interface Iinterface
{
    public void enter();
    public void exit();
}
