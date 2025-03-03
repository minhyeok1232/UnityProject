using System.Collections.Generic;
using System;
using UnityEngine;
public class MonsterInformation : MonoBehaviour            // 게임 몬스터의 정보를 CSV 파일에서 읽어와서 저장해주는 스크립트
{
    // String
    public string monsterName;                             // 몬스터의 이름

    // Class
    public Monster monster;                                // 몬스터에대한 클래스 정보
    public class Monster                               
    {
        public GameObject gameObject { get; set; }         // 몬스터 gameObject에 대한 참조
        public int Number { get; set; }                    // 각 속성들
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Damage { get; set; }
        public int Gold { get; set; }
        public string Name { get; set; }
        public string Item { get; set; }

        // Method
        public void MonsterDamage(TestSlider player)  // PlayerController 스크립트를 참조하여 
        {
            player.PlayerHealth -= this.Damage;             // 캐릭터의 Health값을 해당 오브젝트의 Damage값을 받아서
        }                                                   // Update 해준다.

        private int hp;
        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                gameObject.GetComponent<MonsterBehaviour>().Hp = hp;            // MonsterBehaviour는 해당 몬스터의 HP를 관리하는 클래스라 가정한다.
            }
        }
    }

    private void Awake()
    {
        List<Dictionary<string, object>> MonsterTable = CSVReader.Read("ABCD"); // ABCD라는 csv파일을 읽어서 각 몬스터들에 대한 속성을 Dictionary 형식으로 테이블형태로 저장한다.


        for (int i = 0; i < MonsterTable.Count; i++)
        {
            if (MonsterTable[i]["Name"].ToString() == monsterName)              // 테이블을 싹 훑어보면서 Name 열에 들어간 몬스터이름이,
            {                                                                   // 적 오브젝트 프리팹에 추가한 스크립트 컴퍼넌트 몬스터이름과 같을 시에, 그거에 대한 정보를 가지고와야한다.
                this.monster = new Monster
                {
                    gameObject = this.gameObject,
                    Number = Convert.ToInt32(MonsterTable[i]["Number"]),
                    Name = MonsterTable[i]["Name"].ToString(),
                    Level = Convert.ToInt32(MonsterTable[i]["Level"]),
                    Hp = Convert.ToInt32(MonsterTable[i]["Hp"]),
                    Exp = Convert.ToInt32(MonsterTable[i]["Exp"]),
                    Item = MonsterTable[i]["Item"].ToString(),
                    Damage = Convert.ToInt32(MonsterTable[i]["Damage"]),
                    Gold = Convert.ToInt32(MonsterTable[i]["Gold"])
                };                                                              // 그래서, 각 Number, Name, Level, Hp, Exp, Item, Damage, Gold를 가지고왔다
                break;
            }
        }
    }
}





