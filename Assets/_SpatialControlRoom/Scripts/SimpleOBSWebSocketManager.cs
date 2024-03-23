using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication; // Wichtig f�r ObsDisconnectionInfo
using System;
using UnityEngine;

public class SimpleOBSWebSocketManager : MonoBehaviour
{
    private OBSWebsocket obsWebSocket = new OBSWebsocket();
    [SerializeField] private string serverAdress = "localhost"; // Die URL und der Port k�nnen im UI gesetzt werden
    [SerializeField] private int serverPort = 4444;
    [SerializeField] private string serverPassword = ""; // Das Passwort sollte sicher im UI eingegeben werden

    void Start()
    {
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        obsWebSocket.Connected += OnConnected;
        obsWebSocket.Disconnected += OnDisconnected;

        try
        {
            obsWebSocket.ConnectAsync($"ws://{serverAdress}:{serverPort}", serverPassword);
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket connection error: {e.Message}");
        }
    }

    private void OnConnected(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connection successful");
        // Weitere Initialisierungen nach der Verbindung k�nnen hier erfolgen
    }

    private void OnDisconnected(object sender, ObsDisconnectionInfo e)
    {
        Debug.Log($"Disconnected from OBS WebSocket Server. Reason: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}");
    }

    void OnDestroy()
    {
        if (obsWebSocket.IsConnected)
            obsWebSocket.Disconnect();
    }
}
