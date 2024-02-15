using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Chatting { Player, NPC };
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class QuestInfo : SerializedScriptableObject
{
    // Enum ( ����Ʈ ���� : ������ ������, ���� ���, �ٸ� NPC���� ���ɱ�, Ư�� ��ҷ� �̵� ���... )
    public enum QuestUpdateType
    {
        CollectItem,        // ������ ������
        MonsterHunt,        // ���� ���
        TalkToNPC,          // �ٸ� NPC���� ���ɱ�
        ReachLocation       // Ư�� ��ҷ� �̵�
                            // (���� �� �߰�)
    }
    public QuestUpdateType questType;

    // ����Ʈ ���� (����, ����, Ʃ�丮��...)
    public enum Category { Main, Sub, Tutorial };   // ���� Daily, Weekly, Event �߰�
    public Category category;

    // ����Ʈ�� �Ҵ����ִ� ID (Dictionary�� Key��)
    public int id;
    public string title;

    // ���� ����Ʈ Sequence (�ش�����Ʈ ��������Ʈ ... �������ϱ�)
    public int sequence;

    // ����Ʈ�� ���� ����
    public string description;

    // ����Ʈ ���� ����
    public int requiredLevel;

    // �䱸�ϴ� ����Ʈ
    // 1. ����������Ʈ
    public string itemName;  // �����ؾ� �� �������� �̸�
    public int itemAmount;   // �����ؾ� �� �������� ����

    // 2. ����
    public string MonsterName;
    public int MonsterAmount;

 

    // ����Ʈ �Ϸ�� ����
    public int expReward;
    public Dictionary<Item, int> reward = new Dictionary<Item, int>();

    // ����Ʈ ��ǥ NPC
    public GameObject targetNPC; 

    // Dialogue
    public List<Chat> introductionDialogue = new List<Chat>();    // ����Ʈ �Ұ� ��ȭ

    public List<Chat> acceptedDialogue = new List<Chat>();        // ����Ʈ ���� �� ��ȭ
    public List<Chat> declinedDialogue = new List<Chat>();        // ����Ʈ ���� �� ��ȭ

    public List<Chat> completionDialogue = new List<Chat>();      // ����Ʈ �Ϸ� �� ��ȭ
}  

public class Chat
{
    public Chatting chatting;
    public string ChatText;
}
