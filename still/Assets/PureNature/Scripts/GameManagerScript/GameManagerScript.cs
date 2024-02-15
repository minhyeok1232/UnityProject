using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// Serializable (Skill Data)
[System.Serializable]
public class SkillData
{
    public string skillName;        // ��ų��
    public int skillLevel;          // �ش� ��ų�� ����
    public bool isUnlocked;         // ��ų�� ��ݻ���
}

// Serializable (Game Data)
[System.Serializable] 
public class GameData
{
    public int level;               // ĳ������ ����
    public int experience;          // ĳ������ ����ġ
    public int health;              // ĳ������ HP
    public int mana;                // ĳ������ MP
    public Vector3 playerPosition;  // ĳ������ ��ġ
    public string gold;             // ĳ������ ������ȭ

    // Skill
    public List<SkillData> skills;  // ��ų����
    public int skill_Points;         // ���� ��ų ����Ʈ
    public Dictionary<string, bool> skill_Unlocks; // �� ��ų�� ��� ����
}

// GameManagerScript
public class GameManagerScript : Singleton<GameManagerScript>                       // ������ ���¸� �����ϴ� ���� (Singleton)
{
    public Button GameSetting;      

    public PlayerController player;
    public TestSlider test;
    public CharacterExp exp;

    private DatabaseReference databaseReference;

    // List
    private List<SkillDisplay> skillDisplays;                                       // ��� SkillDisplay ������Ʈ�� ���� ������ ������ ����Ʈ

    // Start
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;         
        skillDisplays = new List<SkillDisplay>(FindObjectsOfType<SkillDisplay>());  // skillDisplays �� "SkillDisplay.cs"�� ������ ��� ������Ʈ���� List���·� �д�.  
    }

    // Method
    public void ButtonQuitGame()                                                    // ���� �α׾ƿ� �޼ҵ�
    {
        if (IsUserLoggedIn())                                                       // �α����� �Ǿ����� ��
        {
            Time.timeScale = 1;                                                     // ȭ���� �ٽ� �����̰� �����, (Time.timeScale = 0�� ȭ���� �����ִ� ����)

            SaveGameState();                                                        // ���� �� ������ ���¸� �����ϸ�,
            FirebaseAuthManager.Instance.LogOut();                                  // �α׾ƿ����·� ���ư���,
            SceneManager.LoadScene("GameStartScene");                               // "GameStartScene" ������ ���ư��� �ȴ�.
        }
    }

    private bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(FirebaseAuthManager.Instance.UserId);          // ����ڰ� �α��εǾ������� True, �ƴϸ� False�� ��ȯ�Ѵ�.
    }

    // ���� ���¸� �����ϴ� �޼���
    private void SaveGameState()
    {
        string userId = FirebaseAuthManager.Instance.UserId;                        // ������� ���̵�� userId�� ���� �Ǹ�,

        GameData data = new GameData                                                // GameData ��ü�� ����
        {
            level = exp.currentLevel,                                               // ���� ����
            experience = exp.currentExp,                                            // ���� ����ġ
            health = test.playerHealth,                                             // ���� ü��
            mana = test.playerMana,                                                 // ���� MP
            playerPosition = player.lastposition,                                   // ���� ��ġ

            skills = new List<SkillData>(),                                         // SkillDataŬ���� ����, ��ų��, ����, ��ݻ��¸� ��Ÿ����.
            skill_Points = SkillManager.instance.User_Skill_Point,                  // ���� ��ų ����Ʈ
            skill_Unlocks = new Dictionary<string, bool>()                          // �� ��ų�� ��� ����
        };

        foreach (var skill in SkillManager.instance.allSkills)                      // SingleTon ������ SkillManager.cs������ <Skill> Ŭ������ ����ϴ� allSkills (��� ��ų��)�� �����Ѵ�.
        {
            data.skills.Add(new SkillData                                           // ��� ��ų �����Ϳ�, 
            {
                skillName = skill.skillName,                                        // ��ų��, ����, ��ݻ��¸� �߰����ش�.
                skillLevel = skill.Skill_Level,
                isUnlocked = skill.isUnlocked
            });
        }

        string jsonData = JsonUtility.ToJson(data);                                 // �����͸� Json ���·� ��ȯ

        // Firebase �����ͺ��̽��� ����
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).ContinueWith(task => {});
    }


    // ���� ���¸� �ε��ϴ� �ڷ�ƾ
    public IEnumerator LoadGameStateCoroutine()
    {
        string userId = FirebaseAuthManager.Instance.UserId;                        // ������� ���̵�� userId�� ���� �Ǹ�,
        if (string.IsNullOrEmpty(userId)) yield break;                              // ���̵� ������ break

        DatabaseReference userRef = databaseReference.Child("users").Child(userId);
        var task = userRef.GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted);

        if (task.IsFaulted)
        {
            Debug.LogError("������ �ε� ����: " + task.Exception);                  // ������ �ε� ����
        }
        else if (task.IsCompleted)                                                  // ������ �ε� ����
        {
            DataSnapshot snapshot = task.Result;                                    // snapshot : ��뵥����
            if (snapshot.Exists)                                                    // ��� �����Ͱ� �����ϴ� ���
            {
                GameData loadedData = JsonUtility.FromJson<GameData>(snapshot.GetRawJsonValue());   // Json���� ��� �����͸� ���´�.
                player.LoadPosition(loadedData.playerPosition);                     // ĳ������ ��ġ��,
                exp.LoadLevel(loadedData.level);                                    // ����
                exp.LoadExp(loadedData.experience);                                 // ����ġ
                test.LoadHp(loadedData.health);                                     // HP
                test.LoadMp(loadedData.mana);                                       // MP

                SkillManager.instance.LoadSkillPoints(loadedData.skill_Points);

                foreach (var skillData in loadedData.skills)                        // Json���� ���� loadedData�� ��� ��ų���� ��ȸ�Ͽ�,
                {
                    Skill skillToUpdate = SkillManager.instance.allSkills.Find(s => s.skillName == skillData.skillName);
                    if (skillToUpdate != null)
                    {
                        skillToUpdate.Skill_Level = skillData.skillLevel;           // ��ų���� ���� �ε�
                        skillToUpdate.isUnlocked = skillData.isUnlocked;            // �ڹ��� ���� �ε�
                    }
                }
            }
            else                                                                    // �� ������� ���
            {
                InitializeDefaultSkills();                                          // ��� ��ų���� 0�������� �ʱ�ȭ�� �����ش�.
            }

            SkillManager.instance.Level_Setting();
        }
    }

    private void InitializeDefaultSkills()                                          // ��� ��ų���� 0������ �ʱ�ȭ �����ִ� �޼ҵ�
    {
        foreach (var skill in SkillManager.instance.allSkills)
        {
            skill.Skill_Level = 0; 
        }
    }

    public void LoadGameState()
    {
        StartCoroutine(LoadGameStateCoroutine());
    }
}
