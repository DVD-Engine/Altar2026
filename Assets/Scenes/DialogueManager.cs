using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    //Assign the Dialogues list in Inspector
    [Header("Dialogues")]
    [SerializeField]
    private Dialogues localDialogues;
    private List<DialogueData> dialogueList;

    [Header("Environment")]
    [SerializeField]
    private EnvironmentManager environmentManager;

    [Header("Radio")]
    [SerializeField]
    private RadioManager radioManager;

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

    [Header("Blip Audio (Animal Crossing style)")]
    [SerializeField] private AudioSource blipAudioSource;
    [SerializeField] private float blipPitchVariance = 0.05f; // que tanto varia el pitch aleatoriamente por caracter

    [Header("Voice Pitch por personaje")]
    [SerializeField] private float teresaPitch = 0.8f;
    [SerializeField] private float reginaPitch = 1.0f;
    [SerializeField] private float laraPitch = 1.3f;

    [Header("Character Stage Sprites")]
    [Tooltip("Asigna aqui el sprite correspondiente a cada etapa de cada personaje")]
    [SerializeField] private List<CharacterStageSprite> characterStageSprites;

    [Header("Character Portrait (UI)")]
    [Tooltip("Image del Canvas donde se muestra el personaje que habla")]
    [SerializeField] private Image characterPortrait;

    private AudioClip blipClip;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    //Variable for referencing the action
    InputAction advanceDialogue;

    //Index for exploring the Dialogue List
    private int currentIndex = 0;

    void Start()
    {
        dialogueList = localDialogues.dialogues;
        environmentManager.InitializeSceneObjects(dialogueList);

        if (radioManager == null)
        {
            Debug.LogWarning("DialogueManager: falta arrastrar el Radio Manager, la radio no va a sonar.");
        }
        advanceDialogue = InputSystem.actions.FindAction("Jump");
        dialogueBox.SetActive(false);
        characterPortrait.enabled = false;

        blipClip = GenerateBlipClip();
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
                environmentManager.HandleSceneObjects(dialogueList[currentIndex]);
                environmentManager.HandleCamera(dialogueList[currentIndex]);
                environmentManager.HandleLighting(dialogueList[currentIndex]);
                environmentManager.HandleAmbientAudio(dialogueList[currentIndex]);
                if (radioManager != null) radioManager.HandleDialogue(dialogueList[currentIndex]);
                HandleCharacterPortrait(dialogueList[currentIndex]);
                HandleDialogueText(dialogueList[currentIndex]);
                currentIndex++;
            }
        }
    }

    public void HandleCharacterPortrait(DialogueData dialogueData)
    {
        // Sin texto: nadie en pantalla
        if (string.IsNullOrEmpty(dialogueData.dialogueText))
        {
            characterPortrait.enabled = false;
            return;
        }

        CharacterStageSprite stageData = GetStageData(dialogueData.characterStage);
        if (stageData == null || stageData.sprite == null)
        {
            characterPortrait.enabled = false;
            return;
        }

        characterPortrait.sprite = stageData.sprite;
        characterPortrait.rectTransform.localScale = Vector3.one * stageData.portraitScale;
        characterPortrait.enabled = true;
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

        typingCoroutine = StartCoroutine(TypeText(dialogueText, dialogueData.TalkingCharacter));
    }

    private IEnumerator TypeText(string dialogueText, Characters speakingChar)
    {
        isTyping = true;

        float basePitch = GetPitchForCharacter(speakingChar);

        charDialogueText.text = dialogueText;
        charDialogueText.ForceMeshUpdate(); // fuerza el recalculo sincronico del mesh
        charDialogueText.maxVisibleCharacters = 0;

        int totalChars = charDialogueText.textInfo.characterCount; // ahora es confiable

        for (int i = 0; i <= totalChars; i++)
        {
            charDialogueText.maxVisibleCharacters = i;

            // Reproduce un blip por cada caracter visible, saltando espacios
            if (i < dialogueText.Length && !char.IsWhiteSpace(dialogueText[i]))
            {
                PlayBlip(basePitch);
            }

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

    // ---------- Character Stage Sprites ----------

    private CharacterStageSprite GetStageData(CharacterStage stage)
    {
        foreach (CharacterStageSprite entry in characterStageSprites)
        {
            if (entry.stage == stage)
            {
                return entry;
            }
        }

        Debug.LogWarning($"No se encontro sprite asignado para la etapa: {stage}");
        return null;
    }

    // ---------- Blip Audio (Animal Crossing style) ----------

    private float GetPitchForCharacter(Characters character)
    {
        switch (character)
        {
            case Characters.Teresa:
                return teresaPitch;
            case Characters.Regina:
                return reginaPitch;
            case Characters.Lara:
                return laraPitch;
            default:
                return 1.0f;
        }
    }

    private void PlayBlip(float basePitch)
    {
        if (blipAudioSource == null || blipClip == null) return;

        blipAudioSource.pitch = basePitch + UnityEngine.Random.Range(-blipPitchVariance, blipPitchVariance);
        blipAudioSource.PlayOneShot(blipClip);
    }

    private AudioClip GenerateBlipClip()
    {
        int sampleRate = 44100;
        float duration = 0.05f; // 50ms, bien cortito
        int sampleCount = (int)(sampleRate * duration);
        float frequency = 440f; // tono base, A4

        AudioClip clip = AudioClip.Create("blip", sampleCount, 1, sampleRate, false);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            // Onda seno con un fade-out rapido para que no truene al final
            float envelope = 1f - (float)i / sampleCount;
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * t) * envelope * 0.5f;
        }

        clip.SetData(samples, 0);
        return clip;
    }
}

[System.Serializable]
public class CharacterStageSprite
{
    public CharacterStage stage;
    public Sprite sprite;
    public float portraitScale = 1f; // para igualar tamanos entre etapas (ej. ninas mas chicas)
}