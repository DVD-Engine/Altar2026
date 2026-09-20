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

    public Positions TalkingCharacterPositions;

    public Characters standingCharacter;

    public Positions StandingCharacterPositions;


    public CameraPosition CameraPosition;

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

public enum Positions
{
    DrawerPos1,
    DrawerPos2,
    CorridorPos1,
    CorridorPos2
}