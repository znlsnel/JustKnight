using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "NewQuest", menuName = "new Quest", order = 1)]
public class QuestSO : ScriptableObject
{
	public string codeName;
	public string displayName;

	[TextArea(3, 10)]
	public string description;

	public QuestTaskSO task;
	// TASK
	// - 카테고리
	// - 이름, 설명
	// Type ( 수를 하나씩 세는건지 거꾸로 세는지 연속적으로 세는지 )
	// target 몬스터, Npc 등등
	// 목표 수

	// REWARD
	public RewardSO reward;
	public bool isCancelable;
	public bool isSavable; 

	
}
