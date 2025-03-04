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
  git clone https://github.com/minhyeok1232/ISAAC_Direct2D.git
```
### 2️⃣ 실행 파일
  Unity Hub 실행 후, 클론한 프로젝트 폴더를 선택 후 "Open" 클릭!
</details>

## 🎯 프로젝트 기간
- MVP 개발 기간 : 2023.03 ~ 2023.11
- 프로젝트 인원 : 1인 (개인)

## 🔗 클래스 다이어그램
### 객체지향 설계를 반영한 클래스 구조도
#### 화면 렌더링 클래스 다이어그램
![image](https://github.com/user-attachments/assets/98746991-15c5-4736-86d8-e3eb87757c21)

#### 오브젝트 & 물리 시스템
![image](https://github.com/user-attachments/assets/b556a03c-a634-40b5-b65b-b075dbcb6a4a)


## 🔄 진행 및 개선 사항
### ✨ 사용자 데이터 저장 시스템

#### 사용자에게 강력한 보안의 데이터 저장을 제공하였습니다.
- Realtime Firebase를 활용하여 클라우드 형식의 데이터 저장 시스템을 구축
- 보안이 취약한 PlayerPrefs 대신 Easy Save 2를 사용하여 데이터 보안 강화
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

### 🤖 서비스 전반의 UI 개선
- 사용자 행동 분석을 통해 게임 UI를 기획하고, 접근성을 개선하여 사용자 경험을 강화하였습니다.
- 다양한 RPG 게임의 UI를 참고하며 직관적이고 효율적인 인터페이스를 설계하였습니다.
#### Before
![image](https://github.com/user-attachments/assets/20d2537d-c945-4fe7-a266-04caf0fd3d61)
#### After
![image](https://github.com/user-attachments/assets/181d38b7-b2a2-4d38-885f-e69656787b47)
<br>
#### 게임 로그인화면? 처음시작할 떄 타이핑효과도 나게했다
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



![image](https://github.com/user-attachments/assets/0e8eab94-d634-4e4e-b7b5-c0f056e7d38c)
1. 정점 설정 및 Shader 참조
- Vertex Shader를 이용하여 충돌 박스를 생성하였습니다.
- DirectX에서 셰이더 코드를 활용하여 정점(Vertices)처리를 하였습니다.
2. HLSL의 World 행렬 적용
- 월드(World), 뷰(View), 프로젝션(Projection) 행렬을 적용하여 충돌 박스를 좌표로 변환하였습니다.
3. HLSL의 Pixel Shader(Color) 적용
- 픽셀 셰이더(Pixel Shader)를 활용하여 충돌 감지 시 색상 변화를 적용하였습니다.
- 충돌 상태에 따라 색상이 변화하도록 셰이더 로직을 구현하였습니다.

<br><br>

## 📝 개발 관점에서의 느낀 점
Win32 API와 C++을 활용하여 게임 엔진을 직접 구현하면서 `저수준 프로그래밍`의 중요성을 체감할 수 있었습니다.

단순한 비트맵 파일을 로드하는 과정조차도 많은 양의 코드가 필요했으며, 윈도우 핸들, 디바이스 컨텍스트 등의 시스템 객체를 다루면서 `하드웨어와`의 `인터페이스`에 대해 이해하게 되었습니다. 
이러한 경험을 통해 시스템 아키텍처와 그래픽 렌더링의 원리를 파악할 수 있었습니다.

이러한 저수준 프로그래밍 경험은 향후 Unity, Unreal Engine과 같은 상용 엔진을 다룰 때 엔진의 내부 동작을 깊이 이해하는 데 도움이 될 것이라 생각합니다. 
또한, 엔진의 기본 구조를 직접 구현해 본 경험이 최적화와 커스텀 렌더링 파이프라인 설계 등의 고급 기술을 익히는 데 중요한 밑바탕이 될 것이라 기대됩니다.

#### 플레이 영상 : [https://www.youtube.com/watch?v=XatDEKotysU](https://www.youtube.com/watch?v=XatDEKotysU)
![Image](https://github.com/user-attachments/assets/dc7bb543-bf0f-438f-9ed1-de4550a6c23a)
