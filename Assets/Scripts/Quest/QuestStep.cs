using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
	// Start is called before the first frame update

	protected void FinishQestStep()
	{

	}
}

// 퀘스트 구성요소
// 제목 설명, 작업(Task), 보상, 아이콘

// Count Task
// Target Category, Count -> 제작소 강화 3번, 던전 탐험 3번, 영웅 소환 5번, 퀘스트정보 확인 1번, NPC와 대화하기 1번

// Taks 구성요소
// 카테고리 - 체크용 string, 이름, Action

// 퀘스트 리포터 
// 카테고리, 타겟(몬스터)가 같은애들 찾아서 카운터

// 몬스터가 죽을때 Action
// -> 퀘스트 리포터와 연결


// 결국 모든 퀘스트는 숫자 세기 놀이
// 퀘스트의 조건이 실행되는 부분에서 (몬스터가 죽거나 하는 부분) 퀘스트 리포트를 실행
// 퀘스트 리포트는 몬스터에게 있음
// 퀘스트  리포트에는 카테고리와 타겟이 지정되어있음
// 해당 카테고리와 타겟을 가진 퀘스트를 찾아서 카운트를 ++해줌


