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

    // Objetos de la escena que cambian en este dialogo (arrastrar desde la jerarquia)
    public List<GameObject> hideObjects = new List<GameObject>();
    public List<GameObject> showObjects = new List<GameObject>();

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