using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTrackerTask : MonoBehaviour
{
	public Text _questDescription;
	public Text _questState;
	 
	/// <summary>
	/// Display in the format [current/target], If 'target' is -1, display [�̿Ϸ�]. 
	/// </summary> 
	public void SetQuestInfo(string description, int cur, int target)
	{
		_questDescription.text = description;
		if (target == -1)
			_questState.text = "[�̿Ϸ�]";
		else if (cur >= target)
			_questState.text = "[�Ϸ�]";
		else
			_questState.text = $"[{cur} / {target}]";
	}


}
