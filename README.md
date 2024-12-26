![Typing SVG](https://readme-typing-svg.demolab.com?font=Fira+Code&size=30&pause=1000&width=435&lines=JUST+KNIGHT)
---
# Description
- **프로젝트 소개** <br>
  해당 프로젝트는 RPG 게임의 기능적 요소들을 직접 구현해보기 위해 시작한 프로젝트입니다. <br>
  NPC와의 대화 시스템, 퀘스트 시스템과 같은 것들을 확장이 용이한 형태로 구현해보는 것을 목표로 개발했습니다.
<br><br>
- **개발 기간** : 2024.07.01 - 2024.09.16
- **사용 기술** <br>
-언어 : C#<br>
-엔진 : Unity Engine <br>
-개발 환경 : Windows 11 <br>
<br>

# 핵심 로직
## 목차
- 퀘스트 시스템 <br>
- NPC 대화 시스템 <br>
- 인벤토리, 아이템 시스템 ,br>
- 저장 기능 <br>
- 카메라 로직 <br>
- 몬스터 생성기

<br> <br>
## 퀘스트 시스템
- 모든 퀘스트 데이터를 Scriptable Object로 관리하여 쉽게 제작이 가능하도록 했습니다. <br>

  ![QuestSO](https://github.com/user-attachments/assets/078e9e25-1e0f-4308-97e6-709bdb28fe73)

- 퀘스트의 내용과 보상을 Scriptable Object로 제작하여 퀘스트를 조립할 수 있게 했습니다.<br>
  ![Quest Task SO](https://github.com/user-attachments/assets/2cf5132d-c6ca-4c6d-8454-6ed77ef74891)

- 퀘스트 진행도를 체크하는 부분은 Quest Reporter에서 담당하도록 했습니다. <br>
  몬스터가 죽을 때, 점프를 할 때, UI를 오픈할 때 등, 진행되는 퀘스트의 카테고리와 타겟을 Key로 하여 진행도를 업데이트 했습니다.<br>
  ![Quest Reporter](https://github.com/user-attachments/assets/a14c4b73-c1f9-4561-a16c-e0b65416936c)


### 퀘스트 UI
- 진행 가능한 퀘스트, 진행중인 퀘스트, 완료된 퀘스트를 확인할 수 있는 UI를 만들었습니다. <br>
  ![Quest UI](https://github.com/user-attachments/assets/97ab28e1-f3ff-44da-a4ee-8bffd37a4c94)

- 체크 버튼을 누르면 플레이 중에도 항상 확인 할 수 있도록 추가 UI를 만들었습니다. <br>
  ![Quest Tracker UI](https://github.com/user-attachments/assets/05b109bf-b9f3-486c-8940-f8e4af4736f4)

 <br> <br>
## NPC 대화 시스템
- 대화를 통해 퀘스트를 받을 수 있는 기능을 만들었습니다. <br>
  Scriptable Obejct를 이용하여 조립하여 만들 수 있게 했습니다.<br>
- 대화를 통해 받게될 퀘스트, 선행 퀘스트를 등록 할 수 있으며, <br>
  퀘스트를 받기 전, 퀘스트 거절 상태, 진행 중, 완료 대기, 완료 후에 따라 다른 대본이 나오도록 설계했습니다. <br><br>
  ![Episode SO](https://github.com/user-attachments/assets/ce1bcc5f-abf9-4208-85d3-2be9c81edbfa)
  <br>

- NPC와의 대화 중 플레이어의 대답에 상태를 부여하여 NPC 대본의 상태가 바뀌도록 했습니다. <br><br>
  ![image (6)](https://github.com/user-attachments/assets/a0f9bb59-ed2f-47d3-a7ae-b323ac4386c5)
  ![대화](https://github.com/user-attachments/assets/12aea5dd-fa07-4d12-a022-ce577b1a26a4)
  

## 인벤토리, 아이템 시스템
- 아이템을 보관하고 장착할 수 있는 UI를 만들었습니다. <br>
  아이템의 정보와 현재 플레이어의 상태를 확인하는 기능을 만들었습니다.<br><br>
![image](https://github.com/user-attachments/assets/6dc61edb-48a6-4cc3-9926-f43028ecb7c6)


<br><br>
## 저장 기능
- 퀘스트 진행 상황, 인벤토리 상태, 모든 NPC들의 상태를 저장하는 기능을 구현했습니다. <br>
  저장하기, 불러오기, 덮어쓰기, 삭제 기능을 만들었습니다.<br><br>
  ![image](https://github.com/user-attachments/assets/2b6bcdac-73bc-41bb-8ef7-2bb4bc65fe80)

  
<br><br>
## 카메라 로직
- 카메라가 특정 영역을 넘어서지 못하도록 하였습니다. 할로우 나이트의 카메라 로직을 따라서 구현했습니다.<br><br>
https://github.com/user-attachments/assets/ee1559ed-eb44-4917-9367-a6c8c8d20996


<br><br>
## 몬스터 생성기
- 빨간 박스 안에서 랜덤한 위치에 몬스터가 생성되도록 하였습니다. <br>
몬스터의 종류, 최대 생성 수, 작동이 시작되고 종료되는 조건(퀘스트)를 설정할 수 있게 했습니다.<br><br>
![image](https://github.com/user-attachments/assets/f883d0bb-3e9e-49cf-aa86-9446eb06948f)






  
