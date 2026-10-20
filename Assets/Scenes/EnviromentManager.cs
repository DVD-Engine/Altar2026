using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    [Header("Stages")]
    [Tooltip("Objeto padre 'Stages': sus hijos directos estan ocultos salvo los que indique cada dialogo")]
    [SerializeField]
    private Transform stagesRoot;

    [Header("Camera:")]
    [SerializeField]
    private Camera targetCamera;

    [Header("Lighting")]
    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private Material materialDay;
    [SerializeField]
    private Material materialSunset;
    [SerializeField]
    private Material materialNight;

    [Header("Ambient Audio")]
    [SerializeField] private AudioSource ambientAudioSource;
    [SerializeField] private AudioClip morningAudio;
    [SerializeField] private AudioClip afternoonAudio;
    [SerializeField] private AudioClip nightAudio;

    private TimeOfDay currentTimeOfDay = (TimeOfDay)(-1); // invalido a proposito, forces first apply
    private TimeOfDay currentAudioTimeOfDay = (TimeOfDay)(-1); // invalido a proposito, fuerza el primer play

    void Start()
    {
        targetCamera.transform.position = new Vector3(0.023f, 1.584f, 1.376f);
        targetCamera.transform.rotation = Quaternion.Euler(6.099f, 0f, 0f);

        RenderSettings.skybox = materialNight;
        sunLight.transform.rotation = Quaternion.Euler(-70.196f, -95.416f, 153.642f);
        sunLight.color = Color.white;
        sunLight.useColorTemperature = true;
        sunLight.colorTemperature = 20000f;
        sunLight.shadowStrength = 0f;
        currentTimeOfDay = TimeOfDay.Night; // keep this in sync so the early-out in HandleLighting behaves correctly
        DynamicGI.UpdateEnvironment();
    }

    // Llamar una sola vez al iniciar: esconde todo lo de Stages y aplica lo que indica el dialogo 0
    public void InitializeSceneObjects(List<DialogueData> allDialogues)
    {
        HideStagesContent();

        if (allDialogues.Count > 0)
        {
            HandleSceneObjects(allDialogues[0]);
        }
    }

    // Esconde todo lo de Stages y muestra solo lo indicado en el dialogo.
    // Lista vacia = no cambia nada.
    public void HandleSceneObjects(DialogueData dialogueData)
    {
        if (dialogueData.showObjects == null || dialogueData.showObjects.Count == 0) return;

        HideStagesContent();

        foreach (GameObject obj in dialogueData.showObjects)
        {
            if (obj == null) continue;

            if (stagesRoot != null && obj.transform.parent != stagesRoot)
            {
                Debug.LogWarning($"'{obj.name}' no es hijo directo de '{stagesRoot.name}', no se escondera en los siguientes dialogos.");
            }

            obj.SetActive(true);
        }
    }

    private void HideStagesContent()
    {
        if (stagesRoot == null) return;

        foreach (Transform child in stagesRoot)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void HandleCamera(DialogueData dialogueData)
    {

        //We get the CameraPosition set from the dialogue
        string cameraPosition = dialogueData.CameraPosition.ToString();

        //If the camera position is Drawer we set the camera pos to this values
        if (cameraPosition == "Drawer")
        {
            targetCamera.transform.position = new Vector3(0.023f, 1.584f, 1.376f);
            targetCamera.transform.rotation = Quaternion.Euler(6.099f, 0f, 0f);
        }

        //If the camera position is Corridor we set the camera pos to this values
        if (cameraPosition == "Corridor")
        {
            targetCamera.transform.position = new Vector3(-0.046f, 1.33f, 1.165f);
            targetCamera.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

    }

    public void HandleLighting(DialogueData dialogueData)
    {
        if (dialogueData.timeOfDay == currentTimeOfDay) return;
        currentTimeOfDay = dialogueData.timeOfDay;

        switch (dialogueData.timeOfDay)
        {
            case TimeOfDay.Day:
                RenderSettings.skybox = materialDay;
                sunLight.transform.rotation = Quaternion.Euler(61.366f, -53.53f, 9.491f);
                sunLight.color = Color.white;
                sunLight.colorTemperature = 6787f;
                sunLight.shadowStrength = 0.691f;
                break;

            case TimeOfDay.Sunset:
                RenderSettings.skybox = materialSunset;
                sunLight.transform.rotation = Quaternion.Euler(20.427f, -24.282f, 50.445f);
                sunLight.color = Color.white;
                sunLight.colorTemperature = 3592f;
                sunLight.shadowStrength = 0.691f;
                break;

            case TimeOfDay.Night:
                RenderSettings.skybox = materialNight;
                sunLight.transform.rotation = Quaternion.Euler(-70.196f, -95.416f, 153.642f);
                sunLight.color = Color.white;
                sunLight.colorTemperature = 20000f;
                sunLight.shadowStrength = 0f;
                break;
        }

        sunLight.useColorTemperature = true;
        DynamicGI.UpdateEnvironment();
    }

    public void HandleAmbientAudio(DialogueData dialogueData)
    {
        // Si el TimeOfDay es igual al que ya esta sonando, no hacemos nada
        if (dialogueData.timeOfDay == currentAudioTimeOfDay) return;

        currentAudioTimeOfDay = dialogueData.timeOfDay;

        AudioClip clipToPlay = null;

        switch (dialogueData.timeOfDay)
        {
            case TimeOfDay.Day:
                clipToPlay = morningAudio;
                break;
            case TimeOfDay.Sunset:
                clipToPlay = afternoonAudio;
                break;
            case TimeOfDay.Night:
                clipToPlay = nightAudio;
                break;
        }

        if (clipToPlay != null)
        {
            ambientAudioSource.clip = clipToPlay;
            ambientAudioSource.loop = true;
            ambientAudioSource.Play();
        }
    }
}