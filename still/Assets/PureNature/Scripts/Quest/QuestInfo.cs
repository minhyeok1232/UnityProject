using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Chatting { Player, NPC };
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class QuestInfo : SerializedScriptableObject
{
    // Enum ( 퀘스트 종류 : 아이템 모으기, 몬스터 사냥, 다른 NPC에게 말걸기, 특정 장소로 이동 등등... )
    public enum QuestUpdateType
    {
        CollectItem,        // 아이템 모으기
        MonsterHunt,        // 몬스터 사냥
        TalkToNPC,          // 다른 NPC에게 말걸기
        ReachLocation       // 특정 장소로 이동
                            // (추후 더 추가)
    }
    public QuestUpdateType questType;

    // 퀘스트 종류 (메인, 서브, 튜토리얼...)
    public enum Category { Main, Sub, Tutorial };   // 추후 Daily, Weekly, Event 추가
    public Category category;

    // 퀘스트에 할당해주는 ID (Dictionary의 Key값)
    public int id;
    public string title;

    // 이전 퀘스트 Sequence (해당퀘스트 다음퀘스트 ... 순서정하기)
    public int sequence;

    // 퀘스트에 대한 설명
    public string description;

    // 퀘스트 레벨 조건
    public int requiredLevel;

    // 요구하는 퀘스트
    // 1. 아이템퀘스트
    public string itemName;  // 수집해야 할 아이템의 이름
    public int itemAmount;   // 수집해야 할 아이템의 갯수

    // 2. 몬스터
    public string MonsterName;
    public int MonsterAmount;

 

    // 퀘스트 완료시 보상
    public int expReward;
    public Dictionary<Item, int> reward = new Dictionary<Item, int>();

    // 퀘스트 목표 NPC
    public GameObject targetNPC; 

    // Dialogue
    public List<Chat> introductionDialogue = new List<Chat>();    // 퀘스트 소개 대화

    public List<Chat> acceptedDialogue = new List<Chat>();        // 퀘스트 수락 후 대화
    public List<Chat> declinedDialogue = new List<Chat>();        // 퀘스트 거절 후 대화

    public List<Chat> completionDialogue = new List<Chat>();      // 퀘스트 완료 후 대화
}  

public class Chat
{
    public Chatting chatting;
    public string ChatText;
}
