using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication; // Important for ObsDisconnectionInfo
using System;
using UnityEngine;

public class OBSWebSocketManager : MonoBehaviour
{
    public OBSWebsocket obsWebSocket { get; private set; }
    [SerializeField] private string serverAddress = "192.168.0.46";
    [SerializeField] private int serverPort = 4455;
    [SerializeField] private string serverPassword = "123123";

    public event Action Connected;
    public event Action Disconnected;

    void Awake()
    {
        obsWebSocket = new OBSWebsocket();
    }

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
            obsWebSocket.ConnectAsync($"ws://{serverAddress}:{serverPort}", serverPassword);
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket connection error: {e.Message}");
        }
    }

    private void OnConnected(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connection successful");
        Connected?.Invoke();
    }

    private void OnDisconnected(object sender, ObsDisconnectionInfo e)
    {
        Debug.Log($"Disconnected from OBS WebSocket Server. Reason: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}");
        Disconnected?.Invoke();
    }

    void OnDestroy()
    {
        if (obsWebSocket.IsConnected)
            obsWebSocket.Disconnect();
    }
}
