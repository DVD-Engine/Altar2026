using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    //Assign references needed in inspector:

    //Assign Camera
    [Header("Camara:")]
    [SerializeField]
    public Camera targetCamera;
    
    //Assign the Dialogues list 
    [Header("Dialogos")]
    [SerializeField]
    public Dialogues localDialogues;




    IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();
        foreach(DialogueData d in localDialogues.dialogues)
        {
            print(d.DialogueID);
        }
    }
}
