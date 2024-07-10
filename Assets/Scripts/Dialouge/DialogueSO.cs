using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum EResponseType
{
	CONTINUE,
	END,
	REJECT,
	GET_REWARD,
	RECEIVE_QUEST
}


[Serializable]
public class RSP
{
	[TextArea(3, 10)]
	public string text;
	public EResponseType state = EResponseType.CONTINUE; 
}


[Serializable]
public class STC 
{
	[TextArea(3, 10)]
	public string text; 
	public List<RSP> player;
}


[CreateAssetMenu(fileName = "new Dialouge", menuName = "Dialogue/new Dialouge", order = 1)]
public class DialogueSO : ScriptableObject
{
	public List<STC> npc; 
}

