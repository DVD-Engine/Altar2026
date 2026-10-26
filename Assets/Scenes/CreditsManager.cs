using System.Collections;
using UnityEngine;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [Header("Canvas aparte (empieza desactivado)")]
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private TextMeshProUGUI creditsText;

    [Header("Texto de los creditos")]
    [TextArea(6, 20)]
    [SerializeField] private string creditsContent =
        "Este altar está dedicado a todas las personas que han fallecido por la falta de medicinas y el mal manejo del sector salud en México.\n\n" +
        "Un proyecto de\nDe Acha Estrada Otto\n[Nombres del equipo]";

    [Header("Maquina de escribir")]
    [SerializeField] private float typeSpeed = 0.05f; // segundos por letra

    void Start()
    {
        if (creditsCanvas != null) creditsCanvas.SetActive(false);
    }

    // Para probar el canvas sin recorrer los dialogos: en Play, tres puntos del componente -> Probar creditos
    [ContextMenu("Probar creditos")]
    private void TestCredits()
    {
        StartCredits();
    }

    // Lo llama DialogueManager cuando ya no hay mas dialogos
    public void StartCredits()
    {
        Debug.Log("CreditsManager: StartCredits llamado");

        if (creditsCanvas == null || creditsText == null)
        {
            Debug.LogError("CreditsManager: faltan arrastrar el Credits Canvas y/o el Credits Text.");
            return;
        }

        creditsCanvas.SetActive(true);
        StartCoroutine(TypeCredits());
    }

    private IEnumerator TypeCredits()
    {
        yield return null; // espera un frame para que el canvas termine de activarse

        creditsText.text = creditsContent;
        creditsText.ForceMeshUpdate();
        creditsText.maxVisibleCharacters = 0;

        int totalChars = creditsText.textInfo.characterCount;

        for (int i = 0; i <= totalChars; i++)
        {
            creditsText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}