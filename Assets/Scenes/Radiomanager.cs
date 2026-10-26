using System.Collections;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    [Header("Radio")]
    [SerializeField] private AudioSource radioSource;

    [Header("Settings")]
    [Tooltip("Volumen que se usa cuando un dialogo pone un clip nuevo sin indicar volumen")]
    [SerializeField, Range(0f, 1f)] private float defaultVolume = 0.3f;
    [Tooltip("Segundos que tarda en cambiar de volumen o de track")]
    [SerializeField] private float fadeSeconds = 1.5f;

    private Coroutine currentRoutine;

    void Awake()
    {
        if (radioSource == null)
        {
            Debug.LogError("RadioManager: falta arrastrar el AudioSource al campo Radio Source.");
            return;
        }

        radioSource.playOnAwake = false;
        radioSource.loop = true;
        radioSource.volume = 0f;
    }

    // Lo llama DialogueManager cada vez que muestra un dialogo
    public void HandleDialogue(DialogueData dialogueData)
    {
        if (radioSource == null) return;

        if (dialogueData.radioClip != null)
        {
            Debug.Log($"Radio: dialogo {dialogueData.DialogueID} pide el clip '{dialogueData.radioClip.name}'");
        }

        if (dialogueData.radioOff)
        {
            StartRoutine(StopRadio());
        }
        else if (dialogueData.radioClip != null)
        {
            StartRoutine(PlayClip(dialogueData.radioClip, dialogueData.radioVolume));
        }
        else if (dialogueData.radioVolume > 0f)
        {
            // Sin clip nuevo: solo cambia el volumen de lo que ya esta sonando
            StartRoutine(FadeTo(dialogueData.radioVolume));
        }
        // Si no hay nada asignado, la radio sigue como estaba
    }

    private void StartRoutine(IEnumerator routine)
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(routine);
    }

    private IEnumerator PlayClip(AudioClip clip, float requestedVolume)
    {
        bool sameClip = radioSource.clip == clip && radioSource.isPlaying;
        float target = requestedVolume > 0f ? requestedVolume : defaultVolume;

        // Mismo clip que ya suena: no se reinicia, solo cambia el volumen si se indico
        if (sameClip)
        {
            if (requestedVolume > 0f) yield return FadeTo(target);
            yield break;
        }

        // Track distinto: baja el que suena (solo si realmente tiene volumen), cambia y sube el nuevo
        if (radioSource.isPlaying && radioSource.volume > 0.01f) yield return FadeTo(0f);

        radioSource.clip = clip;
        radioSource.volume = 0f;
        radioSource.Play();
        yield return FadeTo(target);
    }

    private IEnumerator StopRadio()
    {
        if (radioSource.isPlaying && radioSource.volume > 0.01f) yield return FadeTo(0f);

        radioSource.Stop();
        radioSource.clip = null;
    }

    private IEnumerator FadeTo(float targetVolume)
    {
        if (fadeSeconds <= 0f)
        {
            radioSource.volume = targetVolume;
            yield break;
        }

        float startVolume = radioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeSeconds)
        {
            elapsed += Time.deltaTime;
            radioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / fadeSeconds);
            yield return null;
        }

        radioSource.volume = targetVolume;
    }
}