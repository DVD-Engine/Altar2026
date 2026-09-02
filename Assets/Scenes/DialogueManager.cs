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
            HandleCamera(dialogueList[currentIndex]);
            HandleCharactersPosition(dialogueList[currentIndex]);
            currentIndex++;
        }
    }


    public void HandleCamera(DialogueData dialogueData)
    {

        //We get the CameraPosition set from the dialogue
        string cameraPosition = dialogueData.CameraPosition.ToString();

        //If the camera position is Drawer we set the camera pos to this values
        if(cameraPosition == "Drawer"){
            targetCamera.transform.position = new Vector3(-0.45f, 1f, 0.1f);
            targetCamera.transform.rotation = Quaternion.Euler(0f, 7f, 0f);
        }

        //If the camera position is Corridor we set the camera pos to this values
        if(cameraPosition == "Corridor"){
            targetCamera.transform.position = new Vector3(0f, 1f, 0f);
            targetCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);            
        }

    }

    public void HandleCharactersPosition (DialogueData dialogueData)
    {
        GameObject targetTalkingCharacter = GameObject.Find(dialogueData.TalkingCharacter.ToString());
        GameObject targetStandingCharacter = GameObject.Find(dialogueData.standingCharacter.ToString());

        GameObject targetTalkingCharacterPos = GameObject.Find(dialogueData.TalkingCharacterPositions.ToString());
        GameObject targetStandingCharacterPos = GameObject.Find(dialogueData.StandingCharacterPositions.ToString());

        targetTalkingCharacter.transform.position = targetTalkingCharacterPos.transform.position;
        targetStandingCharacter.transform.position = targetStandingCharacterPos.transform.position;

    }
}
