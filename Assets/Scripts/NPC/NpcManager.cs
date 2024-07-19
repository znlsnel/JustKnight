using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{
	[SerializeField] public List<QuestDialogueSO> _dialogues; 
	QuestDialogueSO _curDialogue;
        public bool isAutoStart = false;
	public bool isOneTimeUse = false;
	public int _beginCnt = 0;
	// Start is called before the first frame update
	void Start()
        {
                _curDialogue = _dialogues[0];

	}

        // Update is called once per frame
        void Update()
        {
        
        }

        //     
        // [ NPC 1] [ NPC 2] [ NPC 3] [ NPC 4] 
	private void OnTriggerEnter2D(Collider2D collision) 
	{

		if ((isOneTimeUse && _beginCnt > 0) || _curDialogue == null || collision.gameObject.GetComponent<PlayerController>() == null)
                        return;

		_beginCnt++;

		if (_curDialogue.GetCurDialogue() == null)
			return;

		InputManager.instance._interactionHandler.AddIAction(gameObject, () => 
		{
                        UIHandler.instance._dialogueSystem.BeginDialogue(_curDialogue);

                        InputManager.instance._interactionHandler.RegisterCancelAction(() => { 
                               UIHandler.instance._dialogueSystem.ActiveMenu(false);  
			});
		});

		if (isAutoStart)
		{
			InputManager.instance._interactionHandler.ExcuteInteraction(); 
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>() == null)
			return;

                InputManager.instance._interactionHandler.Remove(gameObject);
		//Debug.Log("NPC 상호작용 목록에 등록 해제");

	}

}
