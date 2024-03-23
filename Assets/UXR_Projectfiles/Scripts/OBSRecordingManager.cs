using UnityEngine;
using OBSWebsocketDotNet;
using TMPro;

public class OBSRecordingManager : MonoBehaviour
{
    [SerializeField] private OBSWebSocketManager obsWebSocketManager;

    private float timeSinceLastCheck = 0f;
    private float timeSinceLastBackgroundCheck = 0f;
    float checkInterval = 1f; //in seconds
    float backgroundCheckInterval = 20f; //only used to see if changes occur in OBS UI
    private bool isRecording = false;
    private string timecode;

    //public variables
    public bool IsRecording
    {
        get { return isRecording; }
    }

    public string CurrentTimecode
    {
        get { return timecode; }
    }

    void Start()
    {
        obsWebSocketManager.Connected += OnConnected;
        obsWebSocketManager.Disconnected += OnDisconnected;
    }

    void Update()
    {
        if (isRecording)
        {
           
            timeSinceLastCheck += Time.deltaTime;

            if (timeSinceLastCheck >= checkInterval)
            {
                timeSinceLastCheck = 0f;
                CheckRecordingStatus();
            }
        }

        else
        {
            timeSinceLastBackgroundCheck += Time.deltaTime;

            if (timeSinceLastBackgroundCheck >= backgroundCheckInterval)
            {
                timeSinceLastBackgroundCheck = 0f;
                CheckRecordingStatus();
            }
        }
    }

    private void OnConnected()
    {
        Debug.Log("Recording Manager connected to OBS WebSocket");
        CheckRecordingStatus();
    }

    private void OnDisconnected()
    {
        Debug.Log("Recording Manager disconnected from OBS WebSocket");
    }

    public void ToggleRecording()
    {
        try
        {
            if (obsWebSocketManager.obsWebSocket.IsConnected)
            {
                var status = obsWebSocketManager.obsWebSocket.GetRecordStatus();
                if (!status.IsRecording)
                {
                    obsWebSocketManager.obsWebSocket.StartRecord();
                    isRecording = true; 
                }
                else
                {
                    obsWebSocketManager.obsWebSocket.StopRecord();
                    isRecording = false; 
                }
            }
            else
            {
                Debug.LogWarning("Nicht verbunden mit OBS WebSocket. Kann Aufnahme nicht umschalten.");
            }
        }
        catch (ErrorResponseException e)
        {
            Debug.LogError($"Fehler beim Umschalten der Aufnahme: {e.Message}");
            // Hier könntest du eine Benachrichtigung anzeigen oder einen erneuten Versuch planen
        }
    }

    private void CheckRecordingStatus()
    {
        if (obsWebSocketManager.obsWebSocket.IsConnected)
        {
            var status = obsWebSocketManager.obsWebSocket.GetRecordStatus();
            //Debug.Log($"Recording status: {status.IsRecording}, Timecode: {status.RecordTimecode}");
            isRecording = status.IsRecording;

            //changes timecode format from 00:00:00:000 to 00:00:00
            timecode = status.RecordTimecode.Split('.')[0];
        }
        else
        {
            Debug.LogWarning("Cannot check recording status. Not connected to OBS WebSocket.");
        }
    }

    void OnDestroy()
    {
        if (obsWebSocketManager != null && obsWebSocketManager.obsWebSocket != null)
        {
            obsWebSocketManager.Connected -= OnConnected;
            obsWebSocketManager.Disconnected -= OnDisconnected;
        }
    }
}