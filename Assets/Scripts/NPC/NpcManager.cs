using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
        Conversation _script;
       [SerializeField] string _npcName;

    // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //     
        // [ NPC 1] [ NPC 2] [ NPC 3] [ NPC 4] 
	private void OnTriggerEnter2D(Collider2D collision)
	{
                if (collision.gameObject.GetComponent<PlayerController>() == null)
                        return;

                InputManager.instance._interactionHandler.AddIAction(gameObject, () => {

                        UIHandler.instance._dialogueSystem.OpenNPCDialogue(_npcName, 1);

                        Debug.Log("NPC���� ��ȣ�ۿ� ����!");
                        InputManager.instance._interactionHandler.RegisterCancelAction(() => { 
                                UIHandler.instance._dialogueSystem.CloseNPCDialogue(); 
                                Debug.Log("��� Action ��� ! ");
			});
		});
                Debug.Log("NPC ��ȣ�ۿ� ��Ͽ� ���");
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.GetComponent<PlayerController>() == null)
			return;

                InputManager.instance._interactionHandler.Remove(gameObject);
		Debug.Log("NPC ��ȣ�ۿ� ��Ͽ� ��� ����");

	}

}
