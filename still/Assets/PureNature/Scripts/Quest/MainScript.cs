using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ES3Types;
using Sirenix.OdinInspector;
using System.Linq;

public enum ChattingIndex { Player, NPC };
public class MainScript : MonoBehaviour
{
    // Enum ( 대화 종류 : 도입부, 퀘스트 수락이후 대화, 퀘스트 거절이후 대화 )
    public enum DialogueState
    {
        Introduction,   // 도입부
        Accepted,       // 퀘스트 수락
        Declined,       // 퀘스트 거절
        Completed       // 퀘스트 완료 
    }

    public DialogueState currentDialogueState = DialogueState.Introduction;        // 현재 대화는 도입부로 초기화를 시켜준다. 

    // Script
    public PlayerController player;                                                 // 캐릭터에 대한 참조 (대화중에는 캐릭터의 공격 동작을 막는다.)
    public NPCScript currentAcceptedNPC;                                                    // 현재 내가 대화하고 있는 NPC추적(npcScript참조)
    public NPCScript currentChattedNPC;
    public CharacterExp level;                                                      // 캐릭터의 레벨에 검사하는 퀘스트의 조건들...
    public QuestInfo myQuest;                                                       // 특정 퀘스트를 참조하기 위한 변수
    public TestSlider slider;
    public Inventory inventory;

    // GameObject
    public GameObject QuestStartImage;

    private GameObject NPCDialog;                                                   // NPC Dialog
    private GameObject PlayerDialog;                                                // Player Dialog

    public GameObject Accept;                                                       // 수락 버튼
    public GameObject Refuse;                                                       // 거절 버튼

    public GameObject RewardItemImage;
    public GameObject CompletedQuestRewardItemImage;
    public GameObject IsAvailable;

    public GameObject Minimap;



    // Button
    public Button NextButton;                                                       // 다음 버튼 (대화의 다음)
    public Button NPCNextButton;
    // Text
    private Text NPCText;                                                           // NPC의 Text
    private Text PlayerText;                                                        // Player의 Text
    private Text QuestText;

    public Text RewardItemCount;
    public Text RewardExpCount;
    public Text ExecutableQuest;

    // Canvas Group
    public CanvasGroup minimapCanvasGroup;

    // Instance
    public int chatIndex;                                                           // 대화 진행을 위한 변수

    // bool
    private bool isFading = false;

    // Coroutine
    private Coroutine currentTypingCoroutine;                                       

    // Dictionary
    public Dictionary<int, QuestInfo> QuestInfoing = new Dictionary<int, QuestInfo>();               // int(Key값)과 QuestInfo 클래스를 받아주는 QuestInfo의 새로운 객체를 생성한다. 이는 QuestInfo 이다.
    public Dictionary<int, QuestProgress> QuestProgressing = new Dictionary<int, QuestProgress>();   // int(Key값)과 QuestProgress 클래스를 만들어주는 QuestProgress의 새로운 객체를 생성하며, QuestInfo의 Key값과 같아야한다.
    private Dictionary<int, GameObject> npcDialogs = new Dictionary<int, GameObject>();

    public List<QuestInfo> questInfoList = new List<QuestInfo>();                                    // 여러 개의 퀘스트를 저장할 수 있는 리스트
    public List<ChattingText> NotQuestChat = new List<ChattingText>();                               // 퀘스트 소개 대화


    // Start
    private void Start()
    {

        PlayerDialog = GameObject.Find("PlayerDialog");

            // NPCDialog 의 자식에 위치한 NPCText와 PlayerText를 찾아준다. 그러고 각각의 Text 컴퍼넌트를 참조한다.
        PlayerText = PlayerDialog.transform.Find("PlayerText").GetComponent<Text>();
        QuestText = GameObject.Find("QuestText").GetComponent<Text>();

        currentDialogueState = DialogueState.Introduction;      // 도입부부터 대화할 수 있게 초기화

        NPCScript[] allNPCs = FindObjectsOfType<NPCScript>();   // allNPCs 는 NPCScript에 있는 모든 NPC들이다.
        foreach (NPCScript npc in allNPCs)
        {
            npcDialogs[npc.GetInstanceID()] = npc.NPCDialog; // 각 NPC의 대화창을 저장
            npc.NPCDialog.SetActive(false);
        }
        PlayerDialog.SetActive(false);
        Accept.SetActive(false);
        Refuse.SetActive(false);
        CompletedQuestRewardItemImage.SetActive(false);

        LoadQuestData();                                        // 데이터 로드
        QuestStartImage.SetActive(false);
        IsAvailable.SetActive(false);

        minimapCanvasGroup = Minimap.GetComponent<CanvasGroup>();
    }


    //****************************************************************************** 캐릭터와 NPC의 대화 상호작용 ********************************************************************************************
    public void StartFirst(NPCScript npc, QuestInfo quest)
    {
        currentChattedNPC = npc;

        if(npc != null)
        { 
            NPCDialog = npc.NPCDialog;               // NPCDialog 와 PlayerDialog을 찾아준다.
            NPCText = npc.NPCTextUI;
            _prevQuest = npc.questInfo;
        }

        // 현재 대화중인 NPC는 npc로 들어간다.
        player.canAttack = false;                               // NPC와 대화중일 때는, 플레이어의 공격을 비활성화 시켜준다.
        slider.slExp.gameObject.SetActive(false);               // NPC와 대화중일 때는, 경험치바의 표현을 가려준다.



        npc.NPCDialog.SetActive(true);                          // NPC부터 대화를 시작하기 때문에, NPC의 대화창은 켜주며, Player의 대화창은 꺼준다.
        npc.PlayerDialog.SetActive(false);

        NPCNextButton.gameObject.SetActive(false);
        QuestStartImage.SetActive(true);
        IsAvailable.SetActive(true);

        ExecutableQuest.text = quest.title;
        chatIndex = 0;

        ChattingText temp = new ChattingText();
        temp.ChatText = quest.introductionDialogue[chatIndex    ].ChatText;
        temp.chatting = ChattingIndex.NPC;
        ShowDialogue2(temp);

        minimapCanvasGroup.alpha = 0.15f;

    }


    public void StartChat(QuestInfo quest)       // NPC와 Player이 닿게 되면, 채팅을 시작한다.
    {
        player.canAttack = false;                               // NPC와 대화중일 때는, 플레이어의 공격을 비활성화 시켜준다.
        slider.slExp.gameObject.SetActive(false);               // NPC와 대화중일 때는, 경험치바의 표현을 가려준다.

        NPCNextButton.gameObject.SetActive(true);
        QuestStartImage.SetActive(false);
        IsAvailable.SetActive(false);

        minimapCanvasGroup.alpha = 0.15f;


        chatIndex = 0;                                          // 대화의 순서는 0번째부터 시작한다.
        if (QuestProgressing.TryGetValue(quest.id, out QuestProgress progress) && progress.questStatus == QuestProgress.Status.Ending)
        {
            ShowDialogue(quest.completionDialogue[chatIndex]);
            currentDialogueState = DialogueState.Completed;
        }
        else
        {
            ShowDialogue(quest.introductionDialogue[chatIndex]);
        }
        
    }
    // 기본 NPC 대화를 시작하기 위한 StartChat


    // StartChat은 특정 NPC와 닿을 때 대화하기 때문에, NPC에 관한 정보가 있어야 하지만,
    // EndChat은 대화만 끝나면 되기 때문에, NPC에 관한 정보가 필요가 없다.

    public void EndChat()                                       // NPC와 대화를 마무리하는 메소드
    {
        currentChattedNPC.NPCDialog.SetActive(false);                             // Player와 NPC가 상호작용하는 대화UI는 꺼주며,
        PlayerDialog.SetActive(false);

        player.canAttack = true;                                // 플레이어는 대화를 마쳤으니, 공격상태가 가능

        chatIndex = 0;                                          // 채팅 인덱스 초기화

        slider.slExp.gameObject.SetActive(true);

        if(currentDialogueState == DialogueState.Completed)
        {
            CompleteQuest();
        }
        else
            currentDialogueState = DialogueState.Introduction;

        minimapCanvasGroup.alpha = 1.0f;
    }
    private QuestInfo _prevQuest;
    public void NextDialogue(QuestInfo quest)
    {
        chatIndex++;                                                        // 다음대화로 넘어가기 위해서는, 채팅 인덱스 증가s

        switch (currentDialogueState)                                       // 대화상태
        {
            case DialogueState.Introduction:                                // 도입부
                if (chatIndex < _prevQuest.introductionDialogue.Count)           
                {
                    ShowDialogue(_prevQuest.introductionDialogue[chatIndex]);    

                    if (chatIndex >= _prevQuest.introductionDialogue.Count - 1)  // 도입부의 끝은 수락, 거절이 나올 때 까지
                    {
                        Accept.SetActive(true);
                        Refuse.SetActive(true);
                        NextButton.gameObject.SetActive(false);
                    }
                }
                break;

            case DialogueState.Accepted:                                    // 수락상태라면,
                if (chatIndex < _prevQuest.acceptedDialogue.Count)
                {
                    ShowDialogue(_prevQuest.acceptedDialogue[chatIndex]);        // QuestInfo에 퀘스트 수락 이후 대화가 나오며,
                }
                else                                                        // 대화가 마무리 될 시에, 전부 초기화를 시켜준다.
                    EndChat();
                break;

            case DialogueState.Declined:                                    // 거절상태라면,
                if (chatIndex < _prevQuest.declinedDialogue.Count)
                {
                    ShowDialogue(_prevQuest.declinedDialogue[chatIndex]);        // QuestInfo에 퀘스트 거절 이후 대화가 나오며,
                }
                else                                                        // 대화가 마무리 될 시에, 전부 초기화를 시켜준다.
                    EndChat();
                break;

            case DialogueState.Completed:
                if (chatIndex < _prevQuest.completionDialogue.Count)
                {
                    ShowDialogue(_prevQuest.completionDialogue[chatIndex]);
                }
                else
                    EndChat();
                break;
        }
    }


    // 퀘스트 수락 버튼을 누르면, 해당 메소드로 들어가서, currentDialogueState를 Accepted로 변경하게 되고, NextDialogue로 들어가게 된다.
    public void AcceptChatting(QuestInfo quest)
    {
        Accept.SetActive(false);
        Refuse.SetActive(false);
        NextButton.gameObject.SetActive(true);

        currentAcceptedNPC = currentChattedNPC;

        chatIndex = 0;
        ShowDialogue(quest.acceptedDialogue[0]);        // NextDialogue에서 chatIndex를 증가시켜주기 때문에, 0번째 원소에 대한 내용 출력
        currentDialogueState = DialogueState.Accepted;

        InteractWithNPC(currentAcceptedNPC);                    // NPC와 상호작용 진행
    }

    public void RefuseChatting(QuestInfo quest)
    {
        Accept.SetActive(false);
        Refuse.SetActive(false);
        NextButton.gameObject.SetActive(true);

        chatIndex = 0;
        ShowDialogue(quest.declinedDialogue[0]);
        currentDialogueState = DialogueState.Declined;
                                                        // 퀘스트를 거절했기 때문에, 퀘스트 진행중이 아니다.
    }

    public void ShowDialogue(Chat chat)                 // QuestInfo에있는 Chat Class를 자료형으로 받고 chat로 선언)
    {
        if (chat.chatting == Chatting.NPC)              // 채팅하는 대상이 NPC 일시에,
        {
            NPCText.text = chat.ChatText;               // chat 클래스에 ChatText를 NPCText로 들고오며,
            currentChattedNPC.NPCDialog.SetActive(true);                  
            PlayerDialog.SetActive(false);
        }
        else
        {                   
            PlayerText.text = chat.ChatText;            // Player일 시에는, 반대로 적용을 시켜준다.
            PlayerDialog.SetActive(true);
            currentChattedNPC.NPCDialog.SetActive(false);
        }

        if (currentTypingCoroutine != null)             // 현재 코루틴이 null값이 아니라면,
        {                                               // 즉, 어디선가 currentTypingCoroutine을 재 호출하게 된다면
            StopCoroutine(currentTypingCoroutine);      // 코루틴을 끊어주고, null로 만들어준다.
            currentTypingCoroutine = null;
        }

        // 타이핑 효과 코루틴 시작
        Text targetText = (chat.chatting == Chatting.NPC) ? NPCText : PlayerText;   // targetText는 NPC의 채팅인지, Player의 채팅인지 판별한 이후에,
        currentTypingCoroutine = StartCoroutine(Typing(targetText, chat.ChatText)); // 변수를 받아서, 그에 따른 ChatText를 currentTypingCoroutine을 실행한다.
    }
    public void ShowDialogue2(ChattingText chat)                 // QuestInfo에있는 Chat Class를 자료형으로 받고 chat로 선언)
    {
        if (chat.chatting == ChattingIndex.NPC)              // 채팅하는 대상이 NPC 일시에,
        {
            NPCText.text = chat.ChatText;               // chat 클래스에 ChatText를 NPCText로 들고오며,
            currentChattedNPC.NPCDialog.SetActive(true);
            PlayerDialog.SetActive(false);
        }
        else
        {
            PlayerText.text = chat.ChatText;            // Player일 시에는, 반대로 적용을 시켜준다.
            PlayerDialog.SetActive(true);
            currentChattedNPC.NPCDialog.SetActive(false);
        }

        if (currentTypingCoroutine != null)             // 현재 코루틴이 null값이 아니라면,
        {                                               // 즉, 어디선가 currentTypingCoroutine을 재 호출하게 된다면
            StopCoroutine(currentTypingCoroutine);      // 코루틴을 끊어주고, null로 만들어준다.
            currentTypingCoroutine = null;
        }

        // 타이핑 효과 코루틴 시작
        Text targetText = (chat.chatting == ChattingIndex.NPC) ? NPCText : PlayerText;   // targetText는 NPC의 채팅인지, Player의 채팅인지 판별한 이후에,
        currentTypingCoroutine = StartCoroutine(Typing(targetText, chat.ChatText)); // 변수를 받아서, 그에 따른 ChatText를 currentTypingCoroutine을 실행한다.
    }

    IEnumerator Typing(Text targetText, string message)     // 채팅을 키보드치는 형식으로 구현해주는 Typing 코루틴
    {
        targetText.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            targetText.text += message[i];
            yield return new WaitForSeconds(0.05f);
        }
    }



    //****************************************************************************** 캐릭터와 NPC의 대화 상호작용 ********************************************************************************************



    //****************************************************************************** 레벨업 할 때 마다, 수행 가능한 퀘스트 검사 ******************************************************************************

    //******************** 수행 가능한 퀘스트가 생기게 되면, 퀘스트 대화로 이어나가지게 된다. 
    // 1. CheckIsLevelOn 이 True가 되면,
    // 2. NPCScript에 NPC와 닿을 때, CheckIsLevelOn이 True라면,
    // 3. NPCScript에 StartAssignedQuest() 메소드로 가서, 
    // 4. MainScript에 StartChat() 메소드로 가면서,
    // 5. StartChat은 QuestInfo 에 Enum형 (도입부, 퀘스트수락, 퀘스트거절)에 따라 대화가 진행된다.

    public bool CheckQuestRequirementsByLevel()
    {
        bool isQuestAvailable = false; // 변수 추가

        foreach (var quest in QuestInfoing.Values)
        {
            if (!QuestProgressing.ContainsKey(quest.id) && level.currentLevel >= quest.requiredLevel)
            {
                AddQuestInfo(quest.id, quest);
                AddQuestProgress(quest.id, new QuestProgress(QuestProgress.Status.Starting, 0));
                isQuestAvailable = true; // 새로운 퀘스트가 시작되면 true로 설정
            }
        }

        return isQuestAvailable; // true 또는 false 반환
    }

    public void CheckIsLevelOn(out List<QuestInfo> availableQuests, NPCScript npc)                                     
    {
        availableQuests = new List<QuestInfo>();

        foreach (var quest in QuestInfoing.Values)                      // 모든 퀘스트 중에, 하나라도 만족이 되면,,,, True
        {                                                               // 퀘스트의 Key값을 찾아서, 해당 퀘스트에 조건이 만족하는지 구현을 다시 해야 한다. 
            if (level.currentLevel >= quest.requiredLevel)              // level.currentLevel : 현재레벨, quest.requiredLevel : 퀘스트의 요구레벨
            {
                availableQuests.Add(quest);
            }
        }

        if(availableQuests.Count > 0 )
        {
            npc.StartAssignedQuest();
        }
    }

    public bool CanStartQuest(NPCScript npc)                            // 현재 NPC가 퀘스트를 가지고 있는지 없는지
    {
        if (npc == null)
        {
            return false;                                               // NPC가 퀘스트 null값을 들고있으면, false (퀘스트를 줘야함)
        }
        return true;                                                    // NPC가 퀘스트를 들고 있으니, 변화 X
    }




    //****************************************************************************** 레벨업 할 때 마다, 수행 가능한 퀘스트 검사 ******************************************************************************

    //****************************************************************************** 퀘스트를 들고 있는지 검사 ***********************************************************************************************


    // Dictionary 로 구성된 QuestInfoing와 QuestProgressing 는 key값에 따른, value를 들고 온다.
    // QuestInfoing : 주어진 key값에 따른, QuestInfo (클래스 객체 정보 - 퀘스트에 요구 되는 정보)
    // QuestProgressing : 주어진 key값에 따른, QuestProgress (클래스 객체 정보 - 퀘스트 진행 상황)
    // 두 메소드는 key값을 공유하기 때문에, 동시에 움직인다.

    public void AddQuestInfo(int key, QuestInfo info)           
    {
        if (!QuestInfoing.ContainsKey(key))                          // key 값이 없으면,
        {
            QuestInfoing.Add(key, info);                             // 현재 key값에 QuestInfo(퀘스트 정보)를 추가해준다.
        }
    }

    public void AddQuestProgress(int key, QuestProgress progress)    
    {
        if (!QuestProgressing.ContainsKey(key))                      // key 값이 없으면,
        {
            QuestProgressing.Add(key, progress);                     // 현재 key값에 QuestProgress(퀘스트 진행상황)을 추가해준다.
        }
    }

  







    // LoadQuestData, SaveQuestData 추후 검토
   









    public void LoadQuestData()
    {
        // ES3에서 QuestInfoData를 로드하는 대신 인스펙터에서 직접 설정한 questInfoList 사용
        foreach (QuestInfo quest in questInfoList)
        {
            if (!QuestInfoing.ContainsKey(quest.id))
            {
                QuestInfoing.Add(quest.id, quest);
            }
        }

        if (ES3.KeyExists("QuestProgressData"))
        {
            QuestProgressing = ES3.Load<Dictionary<int, QuestProgress>>("QuestProgressData");
        }
        else
        {
            // 필요한 경우 퀘스트 진행 상황에 대한 초기화 코드 추가
        }
    }

    public void SaveQuestData()
    {
        ES3.Save<Dictionary<int, QuestInfo>>("QuestInfoData", QuestInfoing);               // ES3(Easy Save)를 사용하여 Load해준 QuestInfoData와, QuestProgressData를 저장한다.
        ES3.Save<Dictionary<int, QuestProgress>>("QuestProgressData", QuestProgressing);   // 3개모았으면, 현재 3개 모았다고 저장하는 거라고 생각
    }






    // QuestUpdateMethod : 퀘스트를 업데이트해주는 메소드
    // (int) questID : 퀘스트 아이디 
    // (QuestUpdateType) updateType : 위에 선언한 열거형 변수 (아이템 수집, 몬스터 처치, 특정 장소 방문, NPC에게 전달 등등..)
    // (string) paramter = "" : 아이템 수집이면 parameter = 아이템 이름
    //                          몬스터 처치라면 paramater = 몬스터 이름
    //                          특정 장소라면   parameter = 장소 이름 
    // 이런식으로, 해당 이름을 저장하는 string 형식의 매개변수
    public bool QuestUpdateMethod(int questID, QuestInfo.QuestUpdateType updateType, string parameter = "")       
    {
        QuestInfo quest = QuestInfoing[questID];                                // QuestInfo Class 에 있는 퀘스트정보(quest)를 Dictionary에 찾아서 저장
        QuestProgress progress = QuestProgressing[questID];                     // QuestProgress Class에 있는 퀘스트 진행상황(progress)을 Dictionary에 찾아서 저장

        switch (updateType)
        {
            case QuestInfo.QuestUpdateType.CollectItem:                         // 아이템을 수집하는 퀘스트라면,
                if (parameter == quest.itemName)                                // parameter : 내가 수집한 아이템 이름 
                {                                                               // quest.itemName : (고기)로 설정.
                                                                                // 내가 딸기를 주우면 parameter이 딸기로 들어오기 때문에, if문 불충족
                    progress.UpdateProgress(1);                                 // QuestProgress 내부에 있는 UpdateProgress에 Amount값을 1로 받아서, 
                                                                                // 지금상태의 currentProgress(현재 내가 가지고 있는) 값을 1만큼 증가.
                    string Collectmessage = $"{parameter} 수집 상황: {progress.currentProgress} / {quest.itemAmount}";
                    QuestText.text = Collectmessage;
                    StartCoroutine(FadeOutText(QuestText, 3.5f));
                    if (progress.currentProgress >= quest.itemAmount)
                    {
                        return true;
                    }
                }
                break;
            // 몬스터 처치관련 메소드를 추가해도, currentProgress 그대로쓰면 될것같은데...........
            case QuestInfo.QuestUpdateType.MonsterHunt:
                if (parameter == quest.MonsterName)
                {
                    progress.UpdateProgress(1);

                    string Huntmessage = $"{parameter} 처치 상황: {progress.currentProgress} / {quest.itemAmount}";
                    QuestText.text = Huntmessage;
                    StartCoroutine(FadeOutText(QuestText, 3.5f));
                    if (progress.currentProgress >= quest.itemAmount)
                        return true;
                }
                break;
            case QuestInfo.QuestUpdateType.TalkToNPC:
                // NPC와의 대화 업데이트 로직
                break;

            case QuestInfo.QuestUpdateType.ReachLocation:
                // 위치 도착 업데이트 로직
                break;

            // 추가적인 퀘스트 유형에 따른 처리...

            default:
                break;
        }

        return false;
    }

    IEnumerator FadeOutText(Text targetText, float duration)
    {
        if (isFading) yield break; // 이미 코루틴이 실행 중이면 중단
        isFading = true;

        Color originalColor = targetText.color;
        targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1); // 알파값을 1로 재설정

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        isFading = false;
    }
    // MainScript 클래스 내부에 추가될 로직
    public void CompleteQuest()
    {
        CompletedQuestRewardItemImage.SetActive(true);
        player.canAttack = false;

        int questID = currentAcceptedNPC.relatedQuestID;
        QuestInfo quest = QuestInfoing[questID];


        if (quest.reward != null && quest.reward.Count > 0)
        {
            foreach (KeyValuePair<Item, int> rewardPair in quest.reward)
            {
                Item rewardItem = rewardPair.Key;
                int rewardQuantity = rewardPair.Value;

                // 아이템을 받는 로직
                inventory.AcquireItem(rewardItem, rewardQuantity);

                // 아이템 이미지를 설정
                ReceiveRewardItem(rewardItem);

                // 아이템 갯수를 화면에 표시
                RewardItemCount.text = "+ " + rewardQuantity.ToString() + "개";
                RewardExpCount.text = "+ " + quest.expReward.ToString();
            }
        }
  
    }

    public void RewardCompleted()
    {
        int questID = currentAcceptedNPC.relatedQuestID; // NPC에 연결된 퀘스트의 ID를 가져옵니다.

        CompletedQuestRewardItemImage.SetActive(false);

        QuestProgress progress = QuestProgressing[questID];
        // 퀘스트 완료 처리
        progress.questStatus = QuestProgress.Status.Completed;
        RewardPlayer(questID); // 경험치 또는 다른 보상을 플레이어에게 제공하는 함수
        currentDialogueState = DialogueState.Introduction;      // 도입부부터 대화할 수 있게 초기화
        currentAcceptedNPC = null;
    }
    // 보상을 제공하는 함수. 예를 들어 경험치 획득과 같은 로직을 포함
    public void RewardPlayer(int questID)
    {
        QuestInfo quest = QuestInfoing[questID];
        int expReward = quest.expReward;

        level.currentExp += quest.expReward;
    }
    public void ReceiveRewardItem(Item reward)
    {
        itemImageMethod(reward);
    }

    public void itemImageMethod(Item _item)
    {
        Image ImageComponent = RewardItemImage.GetComponent<Image>();
        ImageComponent.sprite = _item.itemImage; // _item의 itemImage 변수에서 이미지 가져오기
    }

    // ActionController.cs 내에 아이템을 줍는 메소드 내에 구현을 해야 한다. (단, 퀘스트가 진행중일 때만)
    public void OnQuestProgressed(int questID, QuestInfo.QuestUpdateType updateType, string parameter = "")           // 퀘스트의 성공 여부를 알기 위한 메소드
    {
        bool isQuestComplete = QuestUpdateMethod(questID, updateType, parameter);                           // QuestUpdateMethod (퀘스트가 업데이트) 될 때 마다 호출해서,
        QuestProgress progress = QuestProgressing[questID];

        if (isQuestComplete)                                                                                // 조건에 맞아서 True를 반환 해주면,
        {
            progress.questStatus = QuestProgress.Status.Ending;
        }
    }

    public void StartQuest(int questID)                                          // 퀘스트ID를 받고 퀘스트를 시작
    {
        QuestInfo quest = QuestInfoing[questID];
        QuestProgress progress = QuestProgressing[questID];

        progress.questStatus = QuestProgress.Status.Proceeding;                  // 퀘스트는 "Proceeding" 상태로 진입
        progress.currentProgress = 0;                                            // 현재 가지고 있는 아이템은 0개
        progress.requiredProgress = quest.itemAmount;                            // 
    }

    public QuestInfo GetQuestInfoById(int id)
    {
        return questInfoList.FirstOrDefault(quest => quest.id == id);
    }
    public void InteractWithNPC(NPCScript npc)
    {
        int questID = npc.relatedQuestID;
        if (QuestInfoing.ContainsKey(questID) && QuestProgressing.ContainsKey(questID))
        {
            QuestInfo quest = QuestInfoing[questID];
            QuestProgress progress = QuestProgressing[questID];

            myQuest = quest; // NPC의 퀘스트를 MyQuest에 할당

            if (myQuest != null)
            {
                StartQuest(questID);
            }
        }
        else if (CanStartQuest(npc))
        {
            QuestInfo quest = npc.questInfo;

            myQuest = quest; // NPC의 퀘스트를 MyQuest에 할당

            AddQuestInfo(questID, quest);
            AddQuestProgress(questID, new QuestProgress(QuestProgress.Status.Starting, 0));
        }
    }
}


//****************************************************************************** QuestProgress 클래스(Dictionary) ******************************************************************************

[System.Serializable]
public class QuestProgress                                      // QuestProgress 클래스
{
    public enum Status { Starting, Proceeding, Ending, Completed }         // 시작가능한지(조건), 진행중인지, 끝난 퀘스트인지 검사를 하며,
    public Status questStatus;                                  // 이 Status 변수를 가져와서 questStatus로 저장한다.
    public int currentProgress;                                 // 현재 진행중인 상황 (지금 내가 얼마나 했는지..)
    public int requiredProgress;                                // 요구되는 상황 ( 총 얼마나 해야 하는지.. )


    public QuestProgress(Status status, int requiredProgress)   // status와 requiredProgress 를 받는 생성자 이며,
    {
        this.questStatus = status;                              // 초기화 작업을 해준다.
        this.currentProgress = 0;
        this.requiredProgress = requiredProgress;
    }

    public void UpdateProgress(int amount)                      // 업데이트를 해주는 역할
    {
        this.currentProgress += amount;                         // int형 amount를 받아서, 현재 진행상황을 더해준다.
    }
}
[System.Serializable]
public class ChattingText
{
    public ChattingIndex chatting;
    public string ChatText;
}