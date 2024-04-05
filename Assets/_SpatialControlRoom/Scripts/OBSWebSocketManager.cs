using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication; // Wichtig für ObsDisconnectionInfo
using OBSWebsocketDotNet.Types;
using OBSWebsocketDotNet.Types.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OBSWebSocketManager : MonoBehaviour
{
    private OBSWebsocket obsWebSocket = new OBSWebsocket();
    [SerializeField] private string serverAdress = "localhost"; // Die URL und der Port können im UI gesetzt werden
    [SerializeField] private int serverPort = 4444;
    [SerializeField] private string serverPassword = ""; // Das Passwort sollte sicher im UI eingegeben werden
    [SerializeField] private UiFeedInstanceManger uiFeedInstanceManger;

    private Queue<Action> actionsToExectuteOnMainThread = new Queue<Action>();
    

    void Start()
    {
        ConnectToServer();
    }

    private void Update()
    {
        while (actionsToExectuteOnMainThread.Count > 0)
        {
            actionsToExectuteOnMainThread.Dequeue().Invoke();
        }
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
        obsWebSocket.CurrentProgramSceneChanged += CurrentProgramSceneChanged;
        obsWebSocket.CurrentPreviewSceneChanged += CurrentPreviewSceneChanged;
        // Weitere Initialisierungen nach der Verbindung können hier erfolgen
    }

    private void OnDisconnected(object sender, ObsDisconnectionInfo e)
    {
        Debug.Log($"Disconnected from OBS WebSocket Server. Reason: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}");
        obsWebSocket.CurrentProgramSceneChanged -= CurrentProgramSceneChanged;
        obsWebSocket.CurrentPreviewSceneChanged -= CurrentPreviewSceneChanged;
    }

    void OnDestroy()
    {
        if (obsWebSocket.IsConnected)
            obsWebSocket.Disconnect();
    }

    //just for testing
    public void StartRecord()
    {
        SendRequest("StartRecord");
    }

    private void SendRequest(string requestType)
    {
        try
        {
            switch (requestType)
            {
                case "StartRecord":
                    obsWebSocket.StartRecord();
                    break;
                default:
                    Debug.LogError($"Unbekannter Request-Typ: {requestType}");
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Fehler beim Senden der Request '{requestType}': {e.Message}");
        }
    }

    private void CurrentProgramSceneChanged(object sender, ProgramSceneChangedEventArgs e)
    {
        //Debug.Log("Current Program Scene: " + e.SceneName);
        actionsToExectuteOnMainThread.Enqueue(() => UpdateUiBasedOnSceneChange("CurrentProgramSceneChanged", e.SceneName));
    }

    private void CurrentPreviewSceneChanged(object sender, CurrentPreviewSceneChangedEventArgs e)
    {
        //Debug.Log("Current Preview Scene: " + e.SceneName);
        actionsToExectuteOnMainThread.Enqueue(() => UpdateUiBasedOnSceneChange("CurrentPreviewSceneChanged", e.SceneName));
    }

    public void UpdateUiBasedOnSceneChange(string callingMethod, string activeScene)
    {
        if (!obsWebSocket.IsConnected)
        {
            Debug.LogError("Not connected to OBS");
            return;
        }

        try
        {
            List<SceneBasicInfo> scenes = obsWebSocket.ListScenes();
            int index = scenes.FindIndex(scene => scene.Name == activeScene);

            if (index != -1)
            {
                uiFeedInstanceManger.UpdateUiFeedScene(index, callingMethod);
                /* if (callingMethod == "CurrentProgramSceneChanged")
                {
                    Debug.Log("Current Program Scene Index: " + index + " Program Scene Name:" + activeScene);

                }

                else if (callingMethod == "CurrentPreviewSceneChanged")
                {
                    Debug.Log("Current Preview Scene Index: " + index + " Preview Scene Name:" + activeScene);
                }*/
            }

            else
            {
                Debug.LogError("Current Scene not found in Scene List");
            }

            /*
            foreach (var scene in scenes)
            {
                Debug.Log("Scene: " + scene.Name);
            }
            */
        }
        catch (Exception e)
        {
            Debug.LogError("Error getting scene list: " + e.Message);
        }
    }
}
