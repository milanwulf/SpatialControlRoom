using UnityEngine;
using TMPro; // Hinzufügen für den Zugriff auf TextMeshPro-Komponenten

public class DebugLogManager : MonoBehaviour
{
    public GameObject logItemPrefab; // Zuweisen im Inspector
    public Transform logContainer; // Zuweisen im Inspector

    private void Awake()
    {
        // Abonniere das LogMessageReceived Ereignis
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Erstelle ein neues Log Item
        GameObject logItemObj = Instantiate(logItemPrefab, logContainer);
        // Ändere dies, um auf die TextMeshPro-Komponente zuzugreifen
        TextMeshProUGUI logText = logItemObj.GetComponentInChildren<TextMeshProUGUI>();
        if (logText != null)
        {
            // Setze den Log Text mit TextMeshPro
            logText.text = logString;
        }
    }

    private void OnDestroy()
    {
        // Vergiss nicht, das Ereignis abzubestellen
        Application.logMessageReceived -= HandleLog;
    }
}
