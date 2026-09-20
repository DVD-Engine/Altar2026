using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    //Assign references needed in inspector:

    //Assign Camera
    [Header("Camera:")]
    [SerializeField]
    private Camera targetCamera;
    
    //Assign the Dialogues list in Inspector
    [Header("Dialogues")]
    [SerializeField]
    private Dialogues localDialogues;
    private List<DialogueData> dialogueList;

    [Header("Dialogue Box")]
    [SerializeField]
    private GameObject dialogueBox;

    [Header("Name in Dialogue Box")]
    [SerializeField]
    private TextMeshProUGUI charName;

    [Header("Text in Dialogue Box")]
    [SerializeField]
    private TextMeshProUGUI charDialogueText;

    [Header("Typewriter Settings")]
    [SerializeField]
    private float typeSpeed = 0.03f; // seconds per character
    
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    //Variable for referencing the action
    InputAction advanceDialogue;

    //Index for exploring the Dialogue List
    private int currentIndex = 0;



    void Start(){

        //We get the dialogue List
        dialogueList = localDialogues.dialogues;  
        //We get the Advance Input Action from the InputSystem
        advanceDialogue = InputSystem.actions.FindAction("Jump");  
        dialogueBox.SetActive(false);

    }

    void Update()
    {
        if(currentIndex > dialogueList.Count - 1)
        {
            Debug.Log("there are no more dialogues");
            return;
        }

        if (advanceDialogue.WasPressedThisFrame())
        {
            if (isTyping)
            {
                // First press while typing: skip straight to full text
                CompleteTyping();
            }
            else
            {
                // Not typing: advance to next dialogue
                Debug.Log(dialogueList[currentIndex].DialogueID.ToString());
                Debug.Log(dialogueList[currentIndex].dialogueText.ToString());

                HandleDialogueBox(dialogueList[currentIndex]);
                HandleCamera(dialogueList[currentIndex]);
                HandleCharactersPosition(dialogueList[currentIndex]);
                HandleDialogueText(dialogueList[currentIndex]);
                currentIndex++;
            }
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

    public void HandleDialogueBox(DialogueData dialogueData)
    {
        string dialogueText = dialogueData.dialogueText;

        if(dialogueText == ""){
            dialogueBox.SetActive(false);
        }
        else{
            dialogueBox.SetActive(true);

        }
    }
    public void HandleDialogueText(DialogueData dialogueData)
    {
        string dialogueText = dialogueData.dialogueText;
        string speakingChar = dialogueData.TalkingCharacter.ToString();

        charName.text = speakingChar;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialogueText));
    }

    private IEnumerator TypeText(string dialogueText)
    {
        isTyping = true;

        charDialogueText.text = dialogueText;
        charDialogueText.ForceMeshUpdate(); // fuerza el recalculo sincrónico del mesh
        charDialogueText.maxVisibleCharacters = 0;

        int totalChars = charDialogueText.textInfo.characterCount; // ahora es confiable

        for (int i = 0; i <= totalChars; i++)
        {
            charDialogueText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
    
        charDialogueText.ForceMeshUpdate();
        charDialogueText.maxVisibleCharacters = charDialogueText.textInfo.characterCount;
        isTyping = false;
        typingCoroutine = null;
    }
}

