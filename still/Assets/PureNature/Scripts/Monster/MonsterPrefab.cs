using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterInformation;

public class MonsterPrefab : MonoBehaviour
{
    // Script
    public CharacterExp characterExp;
    public MainScript main;

    // GameObject
    public GameObject MonsterSpon;
    public GameObject BossMonsterPrefab;

    // Component 
    public Terrain terrain;
    private TerrainData terraindata;

    // Instance
    float Timer = 0.0f;

    // Transform
    private Vector3 terrainPos;
    public int Sponmax = 12;

    // Boolean
    private bool bossSpawned = false;

    // List
    public List<GameObject> enemyList = new List<GameObject>();

    // Start
    void Start()
    {
        terraindata = terrain.terrainData;
        terrainPos = terrain.transform.position;
    }

    // 매 프레임마다 Update
    void Update()
    {
        MonsterSponFunction();
        MonsterRemoveFunction();

        if (characterExp.currentLevel > 3 && !bossSpawned)
        {
            SpawnBossMonster();
            bossSpawned = true; // 보스 몬스터가 이미 생성되었음을 표시
        }
    }

    // Method
    public void MonsterSponFunction()
    {
        float SponTime = 7.0f;                                  // 7초마다 몬스터는 스폰된다.

        Timer += Time.deltaTime;                                // 실제 시간에 따라 Timer 변수 증가

        if (Timer > SponTime && enemyList.Count < Sponmax)      // 최대 스폰 몬스터는 12마리로 설정을 했고,
        {                                                       // 7초가 지나게 되면,
            int CurrentSpon = Sponmax - enemyList.Count;        // CurrentSpon 변수를 선언하며, 이는 앞으로 소환될 몬스터 개수를 나타낸다.

            for (int i = 0; i < CurrentSpon; i++)               // 몬스터 여러마리를 소환하기 위한 for루프문
            {
                Timer = 0.0f;                                   // Timer 를 0초로 변경해줘야 한다. 

                float SponPosX = Random.Range(10f, 55f);        // X, Z 범위는 해당 범위 내 랜덤한 위치
                float SponPosZ = Random.Range(10f, 55.0f);
                float SponPosY = terrain.SampleHeight(new Vector3(SponPosX, 0, SponPosZ)) + terrain.transform.position.y;
                                                                // Y 범위는 terrain Component 내에 있는 SampleHeight Method (Y값을 받아오는)
                                                                // 를 받아와서, X와 Z위치에 있는 Y값을 받아오고, Y값이 0이 아닐 가능성을 위해, 현위치에 있는 y값을 받아와 더하면
                                                                // X와 Z값에 맞춰 terrain 지형에 0값으로 들어오게 된다.
                GameObject monster = Instantiate(MonsterSpon, new Vector3(SponPosX, SponPosY, SponPosZ), Quaternion.identity);
                                                                // MonsterSpon은 GameObject인 Monster Prefab이며, 이 것을 정해진 랜덤 스폰 위치에 생성하고,
                                                                // 회전값을 받지 않기 때문에 Quaternion.identity 옵션을 넣어준다.
                enemyList.Add(monster);                         // 리스트를 받아와, monster를 Add해준다.
            }
        }
    }

    public void MonsterRemoveFunction()                         
    {
        for (int i = enemyList.Count - 1; i >= 0; i--)          // 스폰과 다르게, 제거는 for루프를 반대로 탄다.
        {
            GameObject monster = enemyList[i];                  // MonsterRemoveFunction() 메소드 내에서 선언한 GameObject변수 monster는 해당 조건으로 이 메소드를 
                                                                // 통해 없애야 할 몬스터를 알아야 하기 때문에, 배열로 담았다.
            var monsterBehaviour = monster.GetComponent<MonsterBehaviour>();    // 이 몬스터에 Component로 추가되는 MonsterBehaviour Script를 변수종류에 제약을 두지 않는
                                                                // monsterBehaviour로 선언을 하였다.
               
            if (monsterBehaviour.Hp <= 0)                       // MonsterBehaviour Script에 참조되고 있는 각 monster의 Hp (CSV파일)을 가져와서, 이 값이 0보다 같거나 작을 시
            {
                var monsterInformation = monster.GetComponent<MonsterInformation>();    // 각 몬스터 오브젝트(Prefab)에 붙어있는 Component MonsterInformation 을 가져와서
                characterExp.MonsterKillFunction(monsterInformation.monster);           // 몬스터들의 Hp가 0보다 같거나 작을 시에, CharacterExp Script내에 MonsterKillFunction 메소드를 할당한다. 

                StartCoroutine(monsterBehaviour.DeadState());

                string MonsterName = monsterInformation.monsterName;

                if (main.QuestProgressing.ContainsKey(1))
                {
                    QuestProgress progress = main.QuestProgressing[1];
                    if (progress.questStatus == QuestProgress.Status.Proceeding)
                    {
                        main.OnQuestProgressed(1, QuestInfo.QuestUpdateType.MonsterHunt, MonsterName);
                    }
                }
                if (main.QuestProgressing.ContainsKey(2))
                {
                    QuestProgress progress = main.QuestProgressing[2];
                    if (progress.questStatus == QuestProgress.Status.Proceeding)
                    {
                        main.OnQuestProgressed(2, QuestInfo.QuestUpdateType.MonsterHunt, MonsterName);
                    }
                }
                enemyList.RemoveAt(i);                          // List형식의 enemyList를 제거한다. (이는, Update문에서 계속 실행되는 MonsterSponFunction()메소드를 통해
                                                                // 제거된 List는 7초마다 다시 추가가 된다.
            }
        }
    }
    private void SpawnBossMonster()
    {
        Vector3 bossPosition = new Vector3(50, 1.3f, 39);
        GameObject bossMonster = Instantiate(BossMonsterPrefab, bossPosition, Quaternion.identity);
        enemyList.Add(bossMonster);
    }
}


