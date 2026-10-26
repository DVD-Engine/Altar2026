using System;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    //THE LIST OF ALL THE DIALOGUES IN THE GAME
    [Header("Dialogos:")]
    [SerializeField]
    public List<DialogueData> dialogues = new List<DialogueData>();
}

[System.Serializable]

//What a Dialogue needs to exist
public class DialogueData
{
    public int DialogueID;
    public string dialogueText;
    public Characters TalkingCharacter;
    public CharacterStage characterStage; // que etapa/edad del personaje mostrar en este dialogo
    public CameraPosition CameraPosition;
    public TimeOfDay timeOfDay;

    // Objetos de Stages que se ven desde este dialogo (todo lo demas de Stages se esconde).
    // Vacio = no cambia nada, se queda lo del dialogo anterior.
    public List<GameObject> showObjects = new List<GameObject>();

    // Radio: solo se llena en el dialogo donde algo cambia, en los demas se deja vacio
    [Header("Radio")]
    public AudioClip radioClip;     // vacio = no cambia el audio
    [Range(0f, 1f)]
    public float radioVolume;       // 0 = no cambia el volumen
    public bool radioOff;           // marcar para apagar la radio en este dialogo

}

public enum TimeOfDay
{
    Day,
    Sunset,
    Night
}

//Camera's availble positions
public enum CameraPosition{
    Drawer,
    Corridor
}

//Characters Available in the game
public enum Characters{
    Teresa,
    Regina,
    Lara
}

//Etapas de vida/edad de cada personaje a lo largo de la historia
public enum CharacterStage
{
    // Regina
    Regina_Child,
    Regina_Teenager,
    Regina_Adult,

    // Teresa
    Teresa_Adult,
    Teresa_OlderAdult,

    // Lara
    Lara_Toddler,
    Lara_Child
}