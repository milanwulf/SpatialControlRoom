using UnityEngine;
using OBSWebsocketDotNet;
using TMPro;

public class OBSStreamingManager : MonoBehaviour
{
    /*
    [SerializeField] private OBSWebSocketManager obsWebSocketManager;

    private float timeSinceLastCheck = 0f;
    private float timeSinceLastBackgroundCheck = 0f;
    float checkInterval = 1f; //in seconds
    float backgroundCheckInterval = 20f; //only used to see if changes occur in OBS UI
    private bool isStreaming = false;
    private string timecode;

    //public variables
    public bool IsStreaming
    {
        get { return isStreaming; }
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
        if (isStreaming)
        {
           
            timeSinceLastCheck += Time.deltaTime;

            if (timeSinceLastCheck >= checkInterval)
            {
                timeSinceLastCheck = 0f;
                CheckStreamingStatus();
            }
        }

        else
        {
            timeSinceLastBackgroundCheck += Time.deltaTime;

            if (timeSinceLastBackgroundCheck >= backgroundCheckInterval)
            {
                timeSinceLastBackgroundCheck = 0f;
                CheckStreamingStatus();
            }
        }
    }

    private void OnConnected()
    {
        Debug.Log("Recording Manager connected to OBS WebSocket");
        CheckStreamingStatus();
    }

    private void OnDisconnected()
    {
        Debug.Log("Recording Manager disconnected from OBS WebSocket");
    }

    public void ToggleStreaming()
    {
        try
        {
            if (obsWebSocketManager.obsWebSocket.IsConnected)
            {
                var status = obsWebSocketManager.obsWebSocket.GetStreamStatus();
                if (!status.IsActive)
                {
                    obsWebSocketManager.obsWebSocket.StartStream();
                    isStreaming = true; 
                }
                else
                {
                    obsWebSocketManager.obsWebSocket.StopStream();
                    isStreaming = false; 
                }
            }
            else
            {
                Debug.LogWarning("Not connected with OBS WebSocket. Toggle stream not possible.");
            }
        }
        catch (ErrorResponseException e)
        {
            Debug.LogError($"Error while toggeling stream: {e.Message}");
            // reconnect could be possible here
        }
    }

    private void CheckStreamingStatus()
    {
        if (obsWebSocketManager.obsWebSocket.IsConnected)
        {
            var status = obsWebSocketManager.obsWebSocket.GetStreamStatus();
            //Debug.Log($"Recording status: {status.IsRecording}, Timecode: {status.RecordTimecode}");
            isStreaming = status.IsActive;

            //changes timecode format from 00:00:00:000 to 00:00:00
            timecode = status.TimeCode.Split('.')[0];
        }
        else
        {
            Debug.LogWarning("Cannot check stream status. Not connected to OBS WebSocket.");
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
    */
}