using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ENpcStateType
{
        NONE,
        PENDING,
        COMPLETE,
        TALKING,
}
public class NpcStateUI : MonoBehaviour
{ 
        [SerializeField] TextMeshProUGUI _exclamationMark;
        [SerializeField] TextMeshProUGUI _questionMark;
        [SerializeField] TextMeshProUGUI _talking;  

        public void SetNpcStateUI(QuestDialogueSO dl)
        {
		
	       ENpcStateType type= ENpcStateType.NONE; 
		if (dl.GetCurDialogue(false) != null)
		{
			EDialogueState state = dl._state;
			switch (state)
			{
				case EDialogueState.PENDING_ACCEPTANCE:
				case EDialogueState.REJECTED:
					type = ENpcStateType.PENDING;
					break;

				case EDialogueState.IN_PROGRESS:
				case EDialogueState.COMPLETED:
					type = ENpcStateType.NONE;
					break;

				case EDialogueState.AWAITING_COMPLETION:
					type = ENpcStateType.COMPLETE;
					break;

			}
		} 


		switch (type)
                {
                        case ENpcStateType.NONE:
				_exclamationMark.gameObject.SetActive(false);
				_questionMark.gameObject.SetActive(false);
				_talking.gameObject.SetActive(false);

				break;
                        case ENpcStateType.PENDING:
				_exclamationMark.gameObject.SetActive(true);
				_questionMark.gameObject.SetActive(false);
				_talking.gameObject.SetActive(false);
				break;

                        case ENpcStateType .COMPLETE:
				_exclamationMark.gameObject.SetActive(false); 
				_questionMark.gameObject.SetActive(true); 
				_talking.gameObject.SetActive(false);
				break;

                        case ENpcStateType .TALKING:
				_exclamationMark.gameObject.SetActive(false);
				_questionMark.gameObject.SetActive(false);
				_talking.gameObject.SetActive(true); 
				break;
                }
        }
	
}
