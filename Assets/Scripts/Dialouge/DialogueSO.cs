using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RSP
{
	[TextArea(3, 10)]
	public string text;
	public bool isEnd;
}


[Serializable]
public class STC 
{
	[TextArea(3, 10)]
	public string text; 
	public List<RSP> player;
	public QuestSO quest;
}


[CreateAssetMenu(fileName = "new Dialouge", menuName = "new Dialouge", order = 1)]
public class DialogueSO : ScriptableObject
{
	public Image npcIcon;
	public List<STC> npc; 
}
