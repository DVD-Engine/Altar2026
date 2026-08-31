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
    public string Text;
    public Characters Personaje;

    public CameraPosition CameraPosition;
}

//Camera's availble positions
public enum CameraPosition{
        Mueble,
        Corredor
}

//Characters Available in the game
public enum Characters{
    Teresa,
    Regina,
    Lara
}