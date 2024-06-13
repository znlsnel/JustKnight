using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

struct Dialogue
{
        bool clear;
        List<string> chat;
}

struct Dialogues
{
        int characterID;
        
        List<Dialogue> chats;
}


public class DialogueHandler : MonoBehaviour
{
        // Start is called before the first frame update
        List<Dialogue> dialogues;
         
    void Start()
    { 
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
