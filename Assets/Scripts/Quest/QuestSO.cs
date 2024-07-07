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
	// - ī�װ�
	// - �̸�, ����
	// Type ( ���� �ϳ��� ���°��� �Ųٷ� ������ ���������� ������ )
	// target ����, Npc ���
	// ��ǥ ��

	// REWARD
	public RewardSO reward;
	public bool isCancelable;
	public bool isSavable; 

	
}
