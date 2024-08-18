using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ENpcStateType
{
        NONE,
        QUEST,
        REWARD,
        TALKING,
}

public class NpcStateUI : MonoBehaviour
{ 
        [SerializeField] TextMeshProUGUI _exclamationMark;
        [SerializeField] TextMeshProUGUI _questionMark;
        [SerializeField] TextMeshProUGUI _talking;

	Dictionary<EpisodeSO, ENpcStateType> _states = new Dictionary<EpisodeSO, ENpcStateType>();
	int _questCnt = 0;
	int _rewardCnt = 0;
	int _talkCnt = 0;

	public void SetNpcStateUI(EpisodeSO episode)
        {
		ENpcStateType type = ENpcStateType.NONE;
		if (episode.GetDialogue(false) != null)
		{
			EDialogueState state = episode._state;
			switch (state)
			{
				case EDialogueState.PENDING_ACCEPTANCE:
				case EDialogueState.REJECTED:
					{
						type = ENpcStateType.QUEST;
						break;
					}

				case EDialogueState.IN_PROGRESS:
					{
						type = ENpcStateType.TALKING;
						break;
					}
				case EDialogueState.COMPLETED:
					{
						type = ENpcStateType.NONE;
						break;
					}

				case EDialogueState.AWAITING_COMPLETION:
					{
						type = ENpcStateType.REWARD;
						break;
					}
			}
		}
		if (type == ENpcStateType.QUEST)
			_questCnt++;
		else if (type == ENpcStateType.REWARD)
			_rewardCnt++;
		else if (type == ENpcStateType.TALKING)
			_talkCnt++;

		if (_states.ContainsKey(episode))
		{
			ENpcStateType prev = _states[episode];
			_states[episode] = type;

			if (prev == ENpcStateType.QUEST) 
				_questCnt--;
			else if (prev == ENpcStateType.REWARD)
				_rewardCnt--;
			else if (prev == ENpcStateType.TALKING)
				_talkCnt--;
		}
		else
			_states.Add(episode, type);

		_questionMark.gameObject.SetActive(_rewardCnt > 0);
		_exclamationMark.gameObject.SetActive(_rewardCnt == 0 && _questCnt > 0);
		_talking.gameObject.SetActive(_rewardCnt + _questCnt == 0 && _talkCnt > 0);
	}
	
}
