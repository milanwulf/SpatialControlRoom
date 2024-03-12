using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class OBSWebSocketManager : MonoBehaviour
{
    private WebSocket webSocket;

    [SerializeField] private string obsServerIP; // nur IPv4 funktioniert
    [SerializeField] private int obsServerPort;
    //[SerializeField] private string obsPassword; //aktuell nicht möglich Authentifizierungsprozess nicht implementiert
    [SerializeField] private bool connectAtStart = false;

    void Start()
    {
        // Initiale Konfiguration oder andere Startlogik
        if (connectAtStart)
        {
            ConnectToWebSocket();
        }
    }

    public async void ConnectToWebSocket()
    {
        // Überprüfe, ob bereits eine Verbindung besteht
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            Debug.Log("Es besteht bereits eine Verbindung zum WebSocket-Server.");
            return; // Beende die Methode, um eine erneute Verbindung zu verhindern
        }

        string serverAddress = $"ws://{obsServerIP}:{obsServerPort}";
        Debug.Log("Setze Serveradresse: " + serverAddress);

        webSocket = new WebSocket(serverAddress);

        webSocket.OnOpen += () =>
        {
            Debug.Log("Verbindung geöffnet!");
        };

        webSocket.OnError += (e) =>
        {
            Debug.Log("Fehler! " + e);
        };

        webSocket.OnClose += (e) =>
        {
            Debug.Log("Verbindung geschlossen!");
        };

        webSocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OBS Nachricht: " + message);

            // Prüfen, ob es sich um eine Hello-Nachricht handelt und dann identifizieren
            if (message.Contains("\"op\":0")) // Hello-Nachricht empfangen
            {
                IdentifyWithOBS();
            }
        };

        // Verbindung aufbauen
        await webSocket.Connect();
    }


    private void IdentifyWithOBS()
    {
        // Beispiel für eine Identify-Nachricht ohne Authentifizierung
        string identifyMessage = "{\"op\":1,\"d\":{\"rpcVersion\":1,\"authentication\":\"\",\"eventSubscriptions\":0}}";

        // Sende die Identify-Nachricht an OBS
        SendOBSMessage(identifyMessage);
    }

    void Update()
    {

#if !UNITY_WEBGL || UNITY_EDITOR
        if (webSocket != null)
        {
            webSocket.DispatchMessageQueue();
        }
#endif
    }

    public async void SendOBSMessage(string command)
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            await webSocket.SendText(command);
        }
    }

    private async void OnApplicationQuit()
    {
        if (webSocket != null)
        {
            await webSocket.Close();
        }
    }
}
