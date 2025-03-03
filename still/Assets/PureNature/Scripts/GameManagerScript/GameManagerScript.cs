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
    public string skillName;        // 스킬명
    public int skillLevel;          // 해당 스킬의 레벨
    public bool isUnlocked;         // 스킬의 잠금상태
}

// Serializable (Game Data)
[System.Serializable] 
public class GameData
{
    public int level;               // 캐릭터의 레벨
    public int experience;          // 캐릭터의 경험치
    public int health;              // 캐릭터의 HP
    public int mana;                // 캐릭터의 MP
    public Vector3 playerPosition;  // 캐릭터의 위치
    public string gold;             // 캐릭터의 게임재화

    // Skill
    public List<SkillData> skills;  // 스킬정보
    public int skill_Points;         // 남은 스킬 포인트
    public Dictionary<string, bool> skill_Unlocks; // 각 스킬의 잠금 상태
}

// GameManagerScript
public class GameManagerScript : Singleton<GameManagerScript>                       // 게임의 상태를 관리하는 역할 (Singleton)
{
    public Button GameSetting;      

    public PlayerController player;
    public TestSlider test;
    public CharacterExp exp;

    private DatabaseReference databaseReference;

    // List
    private List<SkillDisplay> skillDisplays;                                       // 모든 SkillDisplay 컴포넌트에 대한 참조를 저장할 리스트

    // Start
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;         
        skillDisplays = new List<SkillDisplay>(FindObjectsOfType<SkillDisplay>());  // skillDisplays 는 "SkillDisplay.cs"가 부착된 모든 오브젝트들을 List형태로 둔다.  
    }

    // Method
    public void ButtonQuitGame()                                                    // 게임 로그아웃 메소드
    {
        if (IsUserLoggedIn())                                                       // 로그인이 되어있을 시
        {
            Time.timeScale = 1;                                                     // 화면을 다시 움직이게 만들며, (Time.timeScale = 0은 화면이 멈춰있는 상태)

            SaveGameState();                                                        // 현재 내 게임의 상태를 저장하며,
            FirebaseAuthManager.Instance.LogOut();                                  // 로그아웃상태로 돌아가고,
            SceneManager.LoadScene("GameStartScene");                               // "GameStartScene" 씬으로 돌아가게 된다.
        }
    }

    private bool IsUserLoggedIn()
    {
        return !string.IsNullOrEmpty(FirebaseAuthManager.Instance.UserId);          // 사용자가 로그인되어있으면 True, 아니면 False를 반환한다.
    }

    // 게임 상태를 저장하는 메서드
    private void SaveGameState()
    {
        string userId = FirebaseAuthManager.Instance.UserId;                        // 사용자의 아이디는 userId로 들어가게 되며,

        GameData data = new GameData                                                // GameData 객체를 생성
        {
            level = exp.currentLevel,                                               // 현재 레벨
            experience = exp.currentExp,                                            // 현재 경험치
            health = test.playerHealth,                                             // 현재 체력
            mana = test.playerMana,                                                 // 현재 MP
            playerPosition = player.lastposition,                                   // 현재 위치

            skills = new List<SkillData>(),                                         // SkillData클래스 내의, 스킬명, 레벨, 잠금상태를 나타낸다.
            skill_Points = SkillManager.instance.User_Skill_Point,                  // 남은 스킬 포인트
            skill_Unlocks = new Dictionary<string, bool>()                          // 각 스킬의 잠금 상태
        };

        foreach (var skill in SkillManager.instance.allSkills)                      // SingleTon 형태의 SkillManager.cs내에서 <Skill> 클래스를 담당하는 allSkills (모든 스킬들)을 순항한다.
        {
            data.skills.Add(new SkillData                                           // 모든 스킬 데이터에, 
            {
                skillName = skill.skillName,                                        // 스킬명, 레벨, 잠금상태를 추가해준다.
                skillLevel = skill.Skill_Level,
                isUnlocked = skill.isUnlocked
            });
        }

        string jsonData = JsonUtility.ToJson(data);                                 // 데이터를 Json 형태로 변환

        // Firebase 데이터베이스에 저장
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(jsonData).ContinueWith(task => {});
    }


    // 게임 상태를 로드하는 코루틴
    public IEnumerator LoadGameStateCoroutine()
    {
        string userId = FirebaseAuthManager.Instance.UserId;                        // 사용자의 아이디는 userId로 들어가게 되며,
        if (string.IsNullOrEmpty(userId)) yield break;                              // 아이디가 없으면 break

        DatabaseReference userRef = databaseReference.Child("users").Child(userId);
        var task = userRef.GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted);

        if (task.IsFaulted)
        {
            Debug.LogError("데이터 로드 실패: " + task.Exception);                  // 데이터 로드 실패
        }
        else if (task.IsCompleted)                                                  // 데이터 로드 성공
        {
            DataSnapshot snapshot = task.Result;                                    // snapshot : 사용데이터
            if (snapshot.Exists)                                                    // 사용 데이터가 존재하는 경우
            {
                GameData loadedData = JsonUtility.FromJson<GameData>(snapshot.GetRawJsonValue());   // Json에서 사용 데이터를 들고온다.
                player.LoadPosition(loadedData.playerPosition);                     // 캐릭터의 위치나,
                exp.LoadLevel(loadedData.level);                                    // 레벨
                exp.LoadExp(loadedData.experience);                                 // 경험치
                test.LoadHp(loadedData.health);                                     // HP
                test.LoadMp(loadedData.mana);                                       // MP

                SkillManager.instance.LoadSkillPoints(loadedData.skill_Points);

                foreach (var skillData in loadedData.skills)                        // Json에서 들고온 loadedData의 모든 스킬들을 순회하여,
                {
                    Skill skillToUpdate = SkillManager.instance.allSkills.Find(s => s.skillName == skillData.skillName);
                    if (skillToUpdate != null)
                    {
                        skillToUpdate.Skill_Level = skillData.skillLevel;           // 스킬레벨 상태 로드
                        skillToUpdate.isUnlocked = skillData.isUnlocked;            // 자물쇠 상태 로드
                    }
                }
            }
            else                                                                    // 새 사용자인 경우
            {
                InitializeDefaultSkills();                                          // 모든 스킬들을 0레벨으로 초기화를 시켜준다.
            }

            SkillManager.instance.Level_Setting();
        }
    }

    private void InitializeDefaultSkills()                                          // 모든 스킬들을 0레벨로 초기화 시켜주는 메소드
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
