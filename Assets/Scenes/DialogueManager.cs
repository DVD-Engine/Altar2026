using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{

    //Assign references needed in inspector:

    //Assign Camera
    [Header("Camara:")]
    [SerializeField]
    public Camera targetCamera;
    
    //Assign the Dialogues list in Inspector
    [Header("Dialogos")]
    [SerializeField]
    public Dialogues localDialogues;
    private List<DialogueData> dialogueList;

    //Variable for referencing the action
    InputAction advanceDialogue;

    //Index for exploring the Dialogue List
    private int currentIndex = 0;



    void Start(){
        //We get the dialogue List
        dialogueList = localDialogues.dialogues;  
        //We get the Advance Input Action from the InputSystem
        advanceDialogue = InputSystem.actions.FindAction("Advance");        
    }

    void Update()
    {
        //If there are no more dialgues to see, we return
        if(currentIndex > dialogueList.Count - 1)
        {
            Debug.Log("there are no more dialogues");
            return;
        }

        //Simple logic to check in which dialogue we are
        if (advanceDialogue.WasPressedThisFrame())
        {
            
            Debug.Log(dialogueList[currentIndex].DialogueID.ToString());
            currentIndex++;
        }
    }


    public void HandleCamera(DialogueData dialogueData)
    {
        //We get the CameraPosition from the dialogue
        string cameraPosition = dialogueData.CameraPosition.ToString();

        if(cameraPosition == "Mueble"){
            transform.position = new Vector3();
        }
        if(cameraPosition == "Corredor"){
            
        }

    }
}
