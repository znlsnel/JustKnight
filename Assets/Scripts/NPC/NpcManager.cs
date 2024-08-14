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
	public bool isEventNpc = false;

	QuestManager _questManager;
	DialogueManager _dialogueManager;
	QuestUI _questUI;
	// Start is called before the first frame update
	void Start()
        {
		_questUI = _questUI = UIHandler.instance._mainMenu.GetComponent<MainMenu>()._questUI;

		_curDialogue = _dialogues[0];
		_questManager = QuestManager.instance;
		_dialogueManager = UIHandler.instance._dialogue.GetComponent<DialogueManager>(); 
		
		foreach (var d in _dialogues)
		{
			QuestManager.instance.AddQuest(d.quest);
			_dialogueManager.AddDialogue(d); 
			_questUI.AddQuest(EQuestMenuType.PENDING, d.quest); 
		}

		if (isEventNpc) 
			GetComponent<SpriteRenderer>().sortingOrder = -1; 
	}

        // Update is called once per frame
        void Update()
        {
        
        }

        //     
        // [ NPC 1] [ NPC 2] [ NPC 3] [ NPC 4] 
	private void OnTriggerEnter2D(Collider2D collision)  
	{ 
		_curDialogue = _dialogueManager.UpdateQuestDialogue(_curDialogue);
		if (_curDialogue.GetCurDialogue() == null || collision.gameObject.GetComponent<PlayerController>() == null)
                        return;

		if (isAutoStart)
		{
			_dialogueManager.BeginDialogue(_curDialogue);
			//InputManager.instance._interactionHandler.ExcuteInteraction();
			return;
		}

		InputManager.instance._interactionHandler.AddIAction(gameObject, () => 
		{
			_dialogueManager.BeginDialogue(_curDialogue);

                        InputManager.instance._interactionHandler.RegisterCancelAction(() => {
				_dialogueManager.ActiveMenu(false);  
			});
		});


	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>() == null)
			return;

                InputManager.instance._interactionHandler.Remove(gameObject);
		//Debug.Log("NPC 상호작용 목록에 등록 해제");

	}

}
