# 숲 속의 작은 전사 : 모험의 시작
Unity 엔진을 활용하여 개발된 Built-In(PC) 3D MMORPG 게임 입니다.

## 📌 목차
1. [🔎 프로젝트 소개](#-프로젝트-소개)
2. [🕒 프로젝트 기간](#-프로젝트-기간)
3. [🔗 클래스 다이어그램](#-클래스-다이어그램)
4. [🔄 진행 및 개선 사항](#-진행-및-개선-사항)
5. [⚡ 프로젝트 최적화 과정](#-프로젝트-최적화-과정)
6. [📝 개발 관점에서의 느낀 점](#-개발-관점에서의-느낀-점)
 
---

## 🔎 프로젝트 소개
- **장르** : Built-In(PC) 3D MMORPG
- **IDE** : Unity 3.18f, Visual Studio 2022
- **목적** : 
  - 사용자에게 몰입감 있는 비주얼을 제공하기 위해 대형 3D Asset으로 제작
  - 사용자에게 역동적인 연출과 부드러운 카메라 움직임을 제공
  - 사용자에게 끊김 없는 플레이 경험을 제공
  - 일관된 시스템 구조와 효율적인 리소스 관리 구현

<details>
  <summary>🎇 프로젝트 실행 방법</summary>

### 1️⃣ Git Clone
  ```bash
  git clone https://github.com/minhyeok1232/UnityProject.git
```
### 2️⃣ 실행 파일
  Unity Hub 실행 후, 클론한 프로젝트 폴더를 선택 후 "Open" 클릭!
</details>

## 🎯 프로젝트 기간
- MVP 개발 기간 : 2023.03 ~ 2023.11
- 프로젝트 인원 : 1인 (개인)

## 🔗 클래스 다이어그램
### 객체지향 설계를 반영한 클래스 구조도
![Image](https://github.com/user-attachments/assets/632683b4-bb61-44c4-97dc-592b0a374de5)

<details>
  <summary> 주요 클래스 설명 </summary>

  #### Character Script :
  - PlayerController : 캐릭터의 메인 스크립트로, 캐릭터의 동작과 관련된 로직을 담당합니다.
  - CharacterExp : 캐릭터의 경험치와 레벨을 관리하는 스크립트입니다.
  - CriticalManager : 크리티컬 데미지를 관리하는 스크립트입니다.

  #### Monster Script :
  - MonsterBehaviour : 몬스터의 메인 스크립트로 몬스터의 동작을 관리합니다.
  - MonsterInformation : CSV 파일을 받아 몬스터의 기본 정보를 관리하는 스크립트입니다.
  - MonsterPrefab : Prefab으로 구성된 몬스터 객체를 관리하는 스크립트입니다.

  #### NPC Script :
  - NPC Script : NPC와 Player의 상호작용을 관리하는 스크립트입니다.
  
  #### Item Script :
  - ActionController: 아이템에 대한 동작을 나타냅니다. (아이템 줍기, 등등 로직)
  - Inventory : 인벤토리를 관리하는 스크립트입니다.
  - Slot : 아이템 관련 데이터를 처리하는 스크립트입니다.
  - DragSlot : 아이템 슬롯의 드래그 기능을 담당합니다.
  
  #### Quest Script :
  - Main Script : NPC와 캐릭터 간의 상호작용을 관리하면서 퀘스트를 진행하는 클래스입니다.
  - QuestInfo : 퀘스트에 대한 정보를 담고 있는 SO(ScriptableObject) 클래스입니다.

  #### Firebase Script :
  - FirebaseAuthManager : Firebase 인증을 관리합니다.
  - GameManagerScript, LoginSystem: 게임과 관련된 로그인 시스템을 관리합니다.

  #### Damage Script :
  - DamageText : 데미지 애니메이션과 텍스트를 표시하는 스크립트입니다.
  - ObjectPooler: 오브젝트 풀링 기능을 최적화하는 스크립트입니다.

  #### Skill Script : 
  - Skill (ScriptableObject): 스킬에 대한 기본 정보를 담고 있는 ScriptableObject 클래스입니다.
  - Skill_CoolTime_Script: 스킬의 쿨타임을 관리하는 스크립트입니다.
  - Skill_Instantiate: 스킬을 Prefab 형태로 구성하여 스킬의 쿨타임을 관리하는 스크립트입니다.
  - Skill_Display: 스킬을 화면에 표시하고, 액션을 트리거하는 스크립트입니다.
  - SkillManager: 스킬을 총괄하고 관리하는 스크립트입니다.
  - SkillSlot: 여러 스킬의 슬롯 기능을 관리하는 스크립트입니다.

</details>


## 🔄 진행 및 개선 사항
### ✨ 사용자 데이터 저장 시스템

#### 사용자에게 강력한 보안의 데이터 저장을 제공하였습니다.
- Realtime Firebase를 활용하여 클라우드 형식의 데이터 저장 시스템을 구축
- 보안이 취약한 PlayerPrefs 대신 Easy Save 3를 사용하여 데이터 보안 강화
![image](https://github.com/user-attachments/assets/7d3dad3a-14c3-4ab0-bcaf-014a790650e4)


### 🔀 카메라 시스템 개선
#### 사용자에게 몰입감 있는 연출을 제공하기 위해 기획 및 구현하였습니다.
- 가상의 카메라 Cinemachine은 유니티 기본 카메라와 독립적으로 작동하기 때문에 간섭이 없습니다.
- 사용자에게 맵 전체를 보여줄 수 있는 Track 형식의 Dolly Camera를 사용하였습니다.
![image](https://github.com/user-attachments/assets/88761052-a507-4d67-8d9c-2c8eb9e00400)
<details>
  <summary>🎇 DollyCam 적용하는 방법 </summary>
    1. Cube는 Mesh를 투명하게 설정을 한 뒤,
    2. Cube에다가 DollyCam을 달아주며, Cube가 움직이는대로 카메라를 촬영
    3. 여기서 Camera는 Cinemachine에 적용된다.
</details>
<br><br>

### 🤖 서비스 전반의 UI 개선
#### 👤 A/B 테스트를 통한 UI 최적화 및 사용자 경험 개선
- 사용자 행동 분석을 통해 게임 UI를 기획하고, 접근성을 개선하여 사용자 경험을 강화하였습니다.
- 다양한 RPG 게임의 UI를 참고하며 직관적이고 효율적인 인터페이스를 설계하였습니다.
#### Before
![image](https://github.com/user-attachments/assets/20d2537d-c945-4fe7-a266-04caf0fd3d61)
#### After
![image](https://github.com/user-attachments/assets/181d38b7-b2a2-4d38-885f-e69656787b47)
<br><br>
#### 🎮 로그인 UI 개선
게임 시작 시 로그인 화면이 표시되며, 타이핑 효과를 활용한 텍스트 애니메이션이 적용시켰습니다.
![Image](https://github.com/user-attachments/assets/fac1bde5-1ed9-4876-97b3-5be9283c88db)

<br><br>

## ⚡ 프로젝트 최적화 과정
### CSV 파일을 이용한 데이터 관리
몬스터 정보를 CSV 파일로 관리하였습니다. 
몬스터 데이터를 monsters.csv에 저장하고, 필요할 때 로드하여 동적으로 생성하도록 변경하였습니다.
CSV 파일을 사용하면 코드를 수정하지 않고도 게임 데이터를 쉽게 변경할 수 있어, 데이터 관리가 유연해집니다.
#### 각 몬스터 개체에 대한 정보(Level, Exp, ... )와 동작(MonsterDamage())을 관리
![image](https://github.com/user-attachments/assets/448bd4ff-c32d-4b99-b06b-b07fd7d8a5a9)

#### 몬스터 정보를 담는 CSV파일
![image](https://github.com/user-attachments/assets/256894bc-bebe-48ba-ba83-164554176e96)

#### CSV파일에서 데이터를 읽고, Monster 객체를 생성
![image](https://github.com/user-attachments/assets/2cbf0732-22aa-4467-b59c-24ce777bcbb8)

### Dictionary를 활용한 데이터 접근
#### Dictionary<int, QuestInfo> 및 Dictionary<int, QuestProgress>를 사용하여 빠른 데이터 검색 가능
![image](https://github.com/user-attachments/assets/556db88d-716f-4c22-b431-6d8f050b49b4)

#### 배열이나 리스트대신 Dictionary를 활용하여 O(1)에 가까운 시간복잡도의 데이터 조회
![image](https://github.com/user-attachments/assets/412d1896-d61a-4a01-b1be-a95d5ef6c761)

### 데이터 저장 및 로드 (ES3 활용)
#### Load Data
![image](https://github.com/user-attachments/assets/d23ce374-7999-4528-8048-ed323eed3ede)
#### Save Data
![image](https://github.com/user-attachments/assets/ca55e674-84aa-4059-a6b3-a5989c837171)
<details>
  <summary>🎇 PlayerPrefs와 ES3의 차이점 </summary>
  
 - PlayerPrefs
   - int, float, string 기본 자료형만 저장 가능하며 복잡한 데이터 구조에는 저장 불가능합니다.
   - 보안 취약하다는 단점이 있습니다.
   - 대량 데이터 저장 시 성능이 저하됩니다.

 - ES3 (Easy Save 3)
   - 객체(Class), Dictionary, List 까지 저장이 가능합니다.
   - 데이터 직렬화를 지원합니다.
   - 클라우드 저장하는 방식이며, AES 암호화 지원을 하기 때문에 보안이 강합니다.
   - 대량의 데이터 저장/로드 부분에서 속도가 빠릅니다. 
  
</details>

<br>

### 이벤트 시스템 적용
![image](https://github.com/user-attachments/assets/0fe3d4ca-275b-44c3-88b1-0cf05e4dd31f)
<br>
'OnQuestCompleted' 이벤트를 활용하여 퀘스트가 완료될 때 외부에서 추가 작업을 할 수 있다.
?.Invoke(myQuest) : 이벤트가 연결된 경우에만 실행하여 안전하게 호출이 가능하다.

![image](https://github.com/user-attachments/assets/0a897c75-0a79-4df3-ab11-1587d5f257be)
<br>
이벤트 기반으로 동작하여 보상 지급, 경험치 지급을 개별적으로 처리할 수 있게 된다.
#### 이는 이벤트 시스템을 통해 추가 기능 확장이 쉬워진다는 장점이 있다.

<br>

### 디자인 패턴을 활용한 시스템 설계
#### 싱글톤(Singleton) 패턴을 사용하여 하나의 인스턴스만 생성하고, 이를 전역적으로 접근할 수 있도록 했습니다.
![image](https://github.com/user-attachments/assets/54a57cb7-c39e-45bc-8b16-b2abb7a2a69b)
>> 메모리와 객체 생성을 최소화하기 때문에 GC(Garbage Collector)의 부담을 줄여줍니다.

#### 오브젝트 풀링
#### 게임 내 동일한 오브젝트를 여러 번 생성하고 파괴하는 비용을 줄이기 위해 오브젝트 풀링(Object Pooling)을 적용했습니다. 
싱글톤 패턴과 오브젝트 풀링을 결합하여, 필요할 때마다 게임 오브젝트를 재사용할 수 있도록 했습니다.
![image](https://github.com/user-attachments/assets/d17f2c7e-59d8-4d8d-a1f2-a2d037961228)

#### 직렬화
MessagePack을 사용하여 JSON보다 빠르고 효율적인 직렬화 방식을 구현했습니다.
JSON은 텍스트 기반 포맷으로 상대적으로 더 많은 데이터를 저장하고 처리하는데 시간이 소요되지만, MessagePack은 이진 포맷을 사용하여 속도, 데이터 크기, 메모리 측면에서 우수한 성능을 보입니다.

#### A/B 테스트 결과
같은 데이터셋을 JSON과 MessagePack으로 각각 직렬화/역직렬화한 후, A/B 테스트를 진행하여 평균 30% 로딩 속도 향상을 확인했습니다.

<details>
  <summary>🎇 JSON와 MessagePack의 차이점 </summary>

  - 속도: MessagePack은 JSON보다 직렬화와 역직렬화 속도가 빠릅니다.
  - 데이터 크기: MessagePack은 이진 포맷을 사용하여 데이터를 더 압축하여 저장합니다.
  - 메모리: MessagePack은 메모리 사용 측면에서도 효율적이며, GC(Garbage Collector)에 미치는 부담을 줄여 성능 향상에 기여합니다.

</details>

<br><br>
유니티는 C#을 기반으로 하여, 에셋과 컴포넌트를 활용해 다양한 기능을 간편하게 구현할 수 있다는 점이 매우 인상적이었습니다. 예를 들어, 애니메이션, 물리 엔진, UI 요소 등을 별도의 코드 작성 없이 에셋만으로 빠르게 추가할 수 있었습니다. 또한, 컴포넌트 기반의 개발 방식 덕분에 각 기능을 독립적으로 관리하고 조합하는 방식으로 복잡한 시스템을 효율적으로 구성할 수 있었습니다.하지만 이와 동시에, 유니티는 많은 기능들이 자동화되어 있기 때문에, 엔진의 내부 동작 원리에 대한 이해가 필요함을 깨달았습니다. 특히, 성능 최적화나 고급 커스터마이징이 필요한 경우, 엔진의 기본 원리를 이해하고 적용하는 것이 중요하다는 점을 느꼈습니다. 이러한 점들이 유니티를 더욱 깊이 있게 활용할 수 있는 열쇠가 될 것이라 생각합니다.
## 📝 개발 관점에서의 느낀 점
유니티는 `C#`을 기반으로 하여 에셋과 컴포넌트를 통해 대부분의 기능을 빠르게 구현할 수 있다는 점이 인상적이었습니다.
예를 들어, 애니메이션, 물리, UI 요소 등 많은 기능을 에셋을 통해 간단히 추가할 수 있었습니다.
또한, 컴포넌트 기반의 개발 방식 덕분에 각 기능을 독립적으로 관리하고 조합하는 방식으로 복잡한 시스템을 효율적으로 구성할 수 있었습니다.

하지만 유니티 엔진은 많은 기능들이 자동화되어 있기 때문에, 엔진의 내부 동작 원리에 대한 이해가 부족하면 한계가 있을 수 있다는 점을 깨달았습니다. 
유니티의 내부 동작 원리와 성능 최적화 기법들을 더 깊이 이해하면서  고급 커스터마이징을 위한 지식들을 쌓아나가고 싶습니다.

#### 플레이 영상 : [https://www.youtube.com/watch?v=H8hn4GONI5I](https://www.youtube.com/watch?v=H8hn4GONI5I)
![Image](https://github.com/user-attachments/assets/aa20c004-7cf4-4eb3-bf5e-b8373a55d9bc)
