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
ISAAC_Direct2D/DirectX2D_ISAAC 경로로 이동
DirectX2D.sln을 실행하여 Visual Studio에서 프로젝트 빌드 및 실행
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
### ✨ 셰이더 프로그래밍 개선
HLSL(Vertex/Pixel Shader)과 행렬(Matrix)를 이용하여 충돌 박스를 구현하였습니다.
- HLSL : Pixel 색상을 변경하여 충돌 상태를 `시각적`으로 표현하였습니다.
- 행렬 : 충돌 박스의 위치, 크기, 회전을 적용하면서 `좌표 변환`을 수행하였습니다.
![Image](https://github.com/user-attachments/assets/e36bf54e-a4c0-4963-867c-379d0b410d94)

### 🔀 멀티 스레드 적용
멀티 스레드를 사용함으로써 게임 시작과 동시에 별도의 스레드에서 모든 씬(Scene)을 미리 로드합니다.
- 기존 싱글스레드 방식 대비 약 `40%` 로딩 시간 단축
- FPS 모니터링 결과 초기 로딩 중 프레임 드랍을 최대 `50%`까지 감소
![Image](https://github.com/user-attachments/assets/563cd473-a65b-491f-b662-d417873f2c67)

### 🤖 AI 시스템 개선
`AStar` 알고리즘을 활용하여 최적의 경로 탐색을 구현하였습니다.
- 격자 시스템으로 적 AI의 이동 가능 영역을 관리
- 실시간으로 경로를 재계산하여 동적 장애물 변화에도 유연하게 대응할 수 있도록 AI 이동 로직을 개선
![Image](https://github.com/user-attachments/assets/2a3a8b97-b556-4194-829d-876009aedce4)

### 💥 충돌 시스템 개선
- AABB(Axis-Aligned Bounding Box) 충돌

X, Y 축에 정렬된 직사각형 충돌 방식으로, 회전이 없으며 빠른 충돌 검사가 가능합니다.
연산이 적어 효율적이라는 장점이 있습니다.
<div align="left"> <img src="https://github.com/user-attachments/assets/b85f1793-ee27-426f-9917-fbb5e7653215" width="550" style="float: left; margin-right: 15px;"> </div>

<br><br>

- OBB(Oriented Bounding Box) 충돌

회전이 고려된 충돌 방식으로, 정확한 충돌 판정 가능합니다.
벡터 연산(내적) 과정으로 연산이 많아 회전이 필요한 오브젝트나 복잡한 충돌 처리가 필요한 경우 사용합니다.
<div align="left"> <img src="https://github.com/user-attachments/assets/de75111d-7c82-4e65-884f-9448e3563e9b" width="800" style="float: left; margin-right: 15px;"> </div>


## ⚡ 프로젝트 최적화 과정
### ImGUI를 통한 실시간 성능 모니터링을 통한 최적화
ImGUI를 활용하여 FPS(Frame Per Second)를 실시간으로 표시하여 성능을 모니터링을 할 수 있게 하였습니다. <br>
![Image](https://github.com/user-attachments/assets/872d2f93-75cb-425d-acc6-a99eff270541)
<br>
디버깅 창을 직접 구현하여 코드 수정 없이 오브젝트를 조정하고 실시간으로 테스트할 수 있는 환경을 구축하였습니다.
![Image](https://github.com/user-attachments/assets/341046f1-ce83-47f2-9182-465c58f0827d)

<br><br>

### HLSL을 활용한 그래픽 최적화
HLSL 언어를 사용한 DirectX 셰이더 프로그래밍을 적용하여 그래픽 렌더링 성능을 최적화하였습니다.
![image](https://github.com/user-attachments/assets/278a6da2-2619-4934-b7fe-93797be35426)

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
