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


# 핵심 기능
## 목차
- 퀘스트 시스템 
- NPC 대화 시스템 
- 인벤토리, 아이템 시스템 
- 저장 기능
- 카메라 로직 
- 몬스터 생성기


<br> <br>
## [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/80b316e0cf75e7aa7345ddb190fb71dd7e533b39/Assets/Scripts/Quest/QuestSO.cs#L16) - 퀘스트 시스템
- 모든 퀘스트 데이터를 Scriptable Object로 관리하여 쉽게 제작이 가능하도록 했습니다. <br> <br>
  ![QuestSO](https://github.com/user-attachments/assets/078e9e25-1e0f-4308-97e6-709bdb28fe73)



- [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/8ca5ed26ff00fc65f089998dd6299097fa5cb696/Assets/Scripts/Quest/Task/QuestTaskSO.cs#L8) - 퀘스트의 내용과 보상을 Scriptable Object로 제작하여 퀘스트를 조립할 수 있게 했습니다.<br> <br>
  ![Quest Task SO](https://github.com/user-attachments/assets/2cf5132d-c6ca-4c6d-8454-6ed77ef74891)


- [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/8ca5ed26ff00fc65f089998dd6299097fa5cb696/Assets/Scripts/Quest/QuestReporter.cs#L14) - 퀘스트 진행도를 체크하는 부분은 Quest Reporter에서 담당하도록 했습니다. <br>
  몬스터가 죽을 때, 점프를 할 때, UI를 오픈할 때 등, 진행되는 퀘스트의 카테고리와 타겟을 Key로 하여 진행도를 업데이트 했습니다.<br><br>
  ![Quest Reporter](https://github.com/user-attachments/assets/a14c4b73-c1f9-4561-a16c-e0b65416936c)


### [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/UI/QuestUI.cs) - 퀘스트 UI
- 진행 가능한 퀘스트, 진행중인 퀘스트, 완료된 퀘스트를 확인할 수 있는 UI를 만들었습니다. <br> <br>
  ![Quest UI](https://github.com/user-attachments/assets/97ab28e1-f3ff-44da-a4ee-8bffd37a4c94)


- [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/92e1e435bf4351d7e6678d17eb5d07000a90ee55/Assets/Scripts/Quest/DisplayQuest/QuestTracker.cs#L6) - 체크 버튼을 누르면 플레이 중에도 항상 확인 할 수 있도록 추가 UI를 만들었습니다. <br> <br>
  ![Quest Tracker UI](https://github.com/user-attachments/assets/05b109bf-b9f3-486c-8940-f8e4af4736f4)

 <br> <br>
 
## [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Dialouge/EpisodeSO.cs) - NPC 대화 시스템
- 대화를 통해 퀘스트를 받을 수 있는 기능을 만들었습니다. <br>
  Scriptable Obejct를 이용하여 조립하여 만들 수 있게 했습니다.<br>
- 대화를 통해 받게될 퀘스트, 선행 퀘스트를 등록 할 수 있으며, <br>
  퀘스트를 받기 전, 퀘스트 거절 상태, 진행 중, 완료 대기, 완료 후에 따라 다른 대본이 나오도록 설계했습니다. <br><br>
  ![Episode SO](https://github.com/user-attachments/assets/ce1bcc5f-abf9-4208-85d3-2be9c81edbfa)
  <br>


- [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/8ca5ed26ff00fc65f089998dd6299097fa5cb696/Assets/Scripts/Dialouge/DialogueSO.cs#L38) - NPC와의 대화 중 플레이어의 대답에 상태를 부여하여 NPC 대본의 상태가 바뀌도록 했습니다. <br><br>
  ![image (6)](https://github.com/user-attachments/assets/a0f9bb59-ed2f-47d3-a7ae-b323ac4386c5)
  ![대화](https://github.com/user-attachments/assets/12aea5dd-fa07-4d12-a022-ce577b1a26a4)


- [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Player/InteractionHandler.cs) - F키를 눌러서 상호작용 할 수 있게 했습니다.<br>
[![Video](https://github.com/user-attachments/assets/40a23027-e7c1-4c1c-bc54-459a1978c1d8)](https://www.youtube.com/watch?time_continue=0&v=WwCU-n6C5Wg&embeds_referring_euri=https%3A%2F%2Fwww.notion.so%2F&source_ve_path=MjM4NTE)
  <br><br>



### [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Dialouge/DialogueStateChecker.cs) - 대화 상태 체커  
- 대화 상태를 체크하는 컴포넌트를 만들어서 상태에 따라 시네마틱과 같은 특정 동작을 실행할 수 있도록 했습니다.<br>
[![Video](https://github.com/user-attachments/assets/24cd93b7-d0fd-41b0-8c3d-cc59dae6d927)](https://www.youtube.com/watch?time_continue=0&v=34veS5yo5cg&embeds_referring_euri=https%3A%2F%2Fwww.notion.so%2F&source_ve_path=MjM4NTE)

<br><br>

##  [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Inventory/InventoryManager.cs) - 인벤토리, 아이템 시스템
- 아이템을 보관하고 장착할 수 있는 UI를 만들었습니다. <br>
  아이템의 정보와 현재 플레이어의 상태를 확인하는 기능을 만들었습니다.<br><br>
![image](https://github.com/user-attachments/assets/d86da71b-b228-4262-ba5e-1bb8be152685)
<br><br>

- [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Inventory/ItemSO.cs) - 아이템도 Scriptable Object를 이용하여 쉽게 제작할 수 있게 했습니다.<br>
  아이템 아이콘, 아이템 설명, 기본 속성을 설정할 수 있게 했습니다. <br>
![image](https://github.com/user-attachments/assets/ae86a655-e324-4c4d-8721-6ead555f68ec)


<br><br>

##  [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Managers/SaveManager.cs) - 저장 기능
- 퀘스트 진행 상황, 인벤토리 상태, 모든 NPC들의 상태를 저장하는 기능을 구현했습니다. <br>
  저장하기, 불러오기, 덮어쓰기, 삭제 기능을 만들었습니다.<br><br>
  ![image](https://github.com/user-attachments/assets/2b6bcdac-73bc-41bb-8ef7-2bb4bc65fe80)

  
<br><br>
## 카메라 로직
- 카메라가 설정해놓은 영역 밖을 넘어서지 못하도록 했습니다.<br>
[![Video](https://github.com/user-attachments/assets/31ec017d-e354-4783-9721-21037d58c1d9)](https://www.youtube.com/watch?time_continue=0&v=RsXFIq6IcdQ&embeds_referring_euri=https%3A%2F%2Fwww.notion.so%2F&source_ve_path=MjM4NTE)


### [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Managers/GameManager.cs) - 씬 전환 페이드 시스템
- 옵저버 패턴을 이용하여 씬이 바뀌면 페이드 인 아웃이 실행 되도록 하였습니다.<br>
[![Video](https://github.com/user-attachments/assets/1e497e8e-1acc-4901-96e0-cb225f01bffb)](https://www.youtube.com/watch?time_continue=0&v=zWQYYEdtQ5o&embeds_referring_euri=https%3A%2F%2Fwww.notion.so%2F&source_ve_path=MjM4NTE)



<br><br>

## [![Link](https://img.shields.io/badge/Link-%23181717.svg?&style=for-the-badge&logo=github&logoColor=white)](https://github.com/znlsnel/JustKnight/blob/main/Assets/Scripts/Monster/MonsterSpawner.cs) - 몬스터 생성기
- 오브젝트 풀링 시스템을 이용하여 몬스터 생성기를 만들었습니다. <br>
- 빨간 박스 안에서 랜덤한 위치에 몬스터가 생성되도록 하였습니다. <br>
몬스터의 종류, 최대 생성 수, 작동이 시작되고 종료되는 조건(퀘스트)을 설정할 수 있게 했습니다.<br><br>
![image](https://github.com/user-attachments/assets/f883d0bb-3e9e-49cf-aa86-9446eb06948f)






  
