using System;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [Header("Dialogos:")]
    [SerializeField]
    public List<DialogueData> dialogues = new List<DialogueData>();
}

[System.Serializable]
public class DialogueData
{
    public int DialogueID;
    public Characters Personaje;

    public CameraPosition CameraPosition;
}

public enum CameraPosition{
        Mueble,
        Corredor
}

public enum Characters{
    Teresa,
    Regina,
    Lara
}