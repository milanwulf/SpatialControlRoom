using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication; // important for ObsDisconnectionInfo
using OBSWebsocketDotNet.Types;
using OBSWebsocketDotNet.Types.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OBSWebSocketManager : MonoBehaviour
{
    //IMPORTANT:
    //Stuff that needs to be executed on the main thread needs to be enqueued in actionsToExectuteOnMainThread
    //Otherwise you will see no change in Ingame when disabling/enabling gameobjects or changing UI elements but Console Logs will work
    //Example: actionsToExectuteOnMainThread.Enqueue(() => WsConnected?.Invoke(true));

    [SerializeField] private UiFeedInstanceManger uiFeedInstanceManger;

    private OBSWebsocket obsWebSocket = new OBSWebsocket();
    private Queue<Action> actionsToExectuteOnMainThread = new Queue<Action>();

    //Development Server Settings
    private string defaultWsAdress = "192.168.0.46";
    private int defaultWsPort = 4455;
    private string defaultWsPassword = "123123";

    //Public Events
    public event Action<bool> WsConnected;
    public event Action<string> WsMessage;
    private string defaultNotConnectedMessage = "No WebSocket connection, please check your settings";

    private void Awake()
    {
        actionsToExectuteOnMainThread = new Queue<Action>();
    }

    void Start()
    {
        AutoConnectToServer();
    }

    private void Update()
    {
        
        while (actionsToExectuteOnMainThread.Count > 0)
        {
            Action action = actionsToExectuteOnMainThread.Dequeue();
            if (action != null)
            {
                action.Invoke();
            }
            else
            {
                Debug.LogWarning($"{nameof(actionsToExectuteOnMainThread)} tryed to do a Null Action");
            }
        }
        
    }
    void OnDestroy()
    {
        if (obsWebSocket.IsConnected)
            obsWebSocket.Disconnect();
    }

    #region Connection Handling
    private void AutoConnectToServer() //called on Startup if PlayerPrefs contain Connection Data
    {
        string serverAdress = PlayerPrefs.GetString("wsAddress", defaultWsAdress);
        int serverPort = PlayerPrefs.GetInt("wsPort", defaultWsPort);
        string serverPassword = PlayerPrefs.GetString("wsPassword", defaultWsPassword);

        if (serverAdress == "" || serverPassword == "")
        {
            Debug.LogWarning("No Connection Data found in PlayerPrefs");
            return;
        }

        ConnectToServer(serverAdress, serverPort, serverPassword);
    }

    public void ConnectToServer(string serverAdress, int serverPort, string serverPassword)
    {
        obsWebSocket.Connected -= OnConnected;  // delete old event listeners before adding new ones
        obsWebSocket.Disconnected -= OnDisconnected;

        obsWebSocket.Connected += OnConnected;
        obsWebSocket.Disconnected += OnDisconnected;

        try
        {
            obsWebSocket.ConnectAsync($"ws://{serverAdress}:{serverPort}", serverPassword);
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket connection error: {e.Message}");
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke($"WebSocket connection error: {e.Message}"));
        }
    }

    private void OnConnected(object sender, EventArgs e)
    {
        actionsToExectuteOnMainThread.Enqueue(() => WsConnected?.Invoke(true));
        actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("WebSocket connection successful"));
        Debug.Log("Connected to OBS");
        obsWebSocket.CurrentProgramSceneChanged += CurrentProgramSceneChanged;
        obsWebSocket.CurrentPreviewSceneChanged += CurrentPreviewSceneChanged;
        obsWebSocket.RecordStateChanged += OBSRecordStateChanged;
        obsWebSocket.StreamStateChanged += OBSStreamStateChanged;

        SetInitRecordingState();
        SetInitStreamingState();
    }

    private void OnDisconnected(object sender, ObsDisconnectionInfo e)
    {
        actionsToExectuteOnMainThread.Enqueue(() => WsConnected?.Invoke(false));
        actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Disconnected from OBS"));
        Debug.Log($"Disconnected from OBS WebSocket Server. Reason: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}");
        obsWebSocket.CurrentProgramSceneChanged -= CurrentProgramSceneChanged;
        obsWebSocket.CurrentPreviewSceneChanged -= CurrentPreviewSceneChanged;
        obsWebSocket.RecordStateChanged -= OBSRecordStateChanged;
        obsWebSocket.StreamStateChanged -= OBSStreamStateChanged;
    }
    #endregion

    #region Scene Switch Handling
    private void CurrentProgramSceneChanged(object sender, ProgramSceneChangedEventArgs e)
    {
        Debug.Log("Current Program Scene: " + e.SceneName);
        actionsToExectuteOnMainThread.Enqueue(() => UpdateUiBasedOnSceneChange("CurrentProgramSceneChanged", e.SceneName));
    }

    private void CurrentPreviewSceneChanged(object sender, CurrentPreviewSceneChangedEventArgs e)
    {
        Debug.Log("Current Preview Scene: " + e.SceneName);
        actionsToExectuteOnMainThread.Enqueue(() => UpdateUiBasedOnSceneChange("CurrentPreviewSceneChanged", e.SceneName));
    }

    public void UpdateUiBasedOnSceneChange(string callingMethod, string activeScene)
    {
        if (!obsWebSocket.IsConnected)
        {
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke(defaultNotConnectedMessage));
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
            }

            else
            {
                Debug.LogError("Current Scene not found in Scene List");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error getting scene list: " + e.Message);
        }
    }

    public void SetPreviewSceneByIndex(int index)
    {
        if (!obsWebSocket.IsConnected)
        {
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke(defaultNotConnectedMessage));
            Debug.LogError("Cant set Preview Scene, not connected to OBS!");
            return;
        }
        try
        {
            List<SceneBasicInfo> scenes = obsWebSocket.ListScenes();
            if (index >= 1 && index <= scenes.Count) //index 0 is ignored because it is the NDI Input Scene
            {
                string sceneName = scenes[index].Name;
                obsWebSocket.SetCurrentPreviewScene(sceneName);
            }

            else
            {
                Debug.LogError("Invalid scene Index");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error setting preview scene: " + e.Message);
        }
    }
    #endregion

    #region Transition Handling
    public void TriggerStudioModeTransition()
    {
        if (!obsWebSocket.IsConnected)
        {
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke(defaultNotConnectedMessage));
            Debug.LogError("Cant trigger Studio Mode Transition, not connected to OBS!");
            return;
        }

        if(!obsWebSocket.GetStudioModeEnabled())
        {
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Please enable Studio Mode in OBS"));
            Debug.LogError("Studio Mode is not enabled");
            return;
        }

        try
        {
            obsWebSocket.TriggerStudioModeTransition();
        }
        catch (Exception e)
        {
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Unable to trigger transition: " + e.Message));
            Debug.LogError("Error triggering Studio Mode Transition: " + e.Message);
        }
    }
    #endregion

    #region Recording Handling
    public bool IsRecording { get; private set; }
    public event Action<bool> RecordingState;

    private void SetInitRecordingState()
    {
        var recordingStatus = obsWebSocket.GetRecordStatus();
        IsRecording = recordingStatus.IsRecording;
        Debug.Log("Initial Recording State: " + IsRecording);
        actionsToExectuteOnMainThread.Enqueue(() => RecordingState?.Invoke(IsRecording));
    }

    private void OBSRecordStateChanged(object sender, RecordStateChangedEventArgs e)
    {
        string unfilteredState = e.OutputState.StateStr; //needed to distinguish between OBS_WEBSOCKET_OUTPUT_STARTING / OBS_WEBSOCKET_OUTPUT_STARTED and OBS_WEBSOCKET_STOPPING / OBS_WEBSOCKET_OUTPUT_STOPPED / OBS_WEBSOCKET_OUTPUT_PAUSED

        if(unfilteredState == "OBS_WEBSOCKET_OUTPUT_STARTED")
        {
            IsRecording = true;
            actionsToExectuteOnMainThread.Enqueue(() => RecordingState?.Invoke(IsRecording));
            Debug.Log("Recording State Changed: " + IsRecording);
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Recording has been started"));
        }

        else if (unfilteredState == "OBS_WEBSOCKET_OUTPUT_STOPPED")
        {
            IsRecording = false;
            actionsToExectuteOnMainThread.Enqueue(() => RecordingState?.Invoke(IsRecording));
            Debug.Log("Recording State Changed: " + IsRecording);
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Recording has been stopped"));
        }
    }

    public void ToggleRecording()
    {
        if (!obsWebSocket.IsConnected)
        {
            Debug.LogError("Cannot toggle recording, not connected to OBS!");
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke(defaultNotConnectedMessage));
            return;
        }

        try
        {
            var recoringStatus = obsWebSocket.GetRecordStatus();
            if (recoringStatus.IsRecording)
            {
                obsWebSocket.StopRecord();
            }
            else
            {
                obsWebSocket.StartRecord();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error toggling recording: " + e.Message);
        }
    }
    #endregion

    #region Stream Handling
    public bool IsStreaming { get; private set; }
    public event Action<bool> StreamingState;

    private void SetInitStreamingState()
    {
        var streamingStatus = obsWebSocket.GetStreamStatus();
        IsStreaming = streamingStatus.IsActive;
        Debug.Log("Initial Streaming State: " + IsStreaming);
        actionsToExectuteOnMainThread.Enqueue(() => StreamingState?.Invoke(IsStreaming));
    }

    private void OBSStreamStateChanged(object sender, StreamStateChangedEventArgs e)
    {
        string unfilteredState = e.OutputState.StateStr; //needed to distinguish between OBS_WEBSOCKET_OUTPUT_STARTING / OBS_WEBSOCKET_OUTPUT_STARTED and OBS_WEBSOCKET_STOPPING / OBS_WEBSOCKET_OUTPUT_STOPPED / OBS_WEBSOCKET_OUTPUT_PAUSED
        if (unfilteredState == "OBS_WEBSOCKET_OUTPUT_STARTED")
        {
            IsStreaming = true;
            actionsToExectuteOnMainThread.Enqueue(() => StreamingState?.Invoke(IsStreaming));
            Debug.Log("Recording State Changed: " + IsStreaming);
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Stream has been started"));
        }

        else if (unfilteredState == "OBS_WEBSOCKET_OUTPUT_STOPPED")
        {
            IsStreaming = false;
            actionsToExectuteOnMainThread.Enqueue(() => StreamingState?.Invoke(IsStreaming));
            Debug.Log("Recording State Changed: " + IsStreaming);
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke("Stream has been stopped"));
        }
    }

    public void ToggleStreaming()
    {
        if (!obsWebSocket.IsConnected)
        {
            actionsToExectuteOnMainThread.Enqueue(() => WsMessage?.Invoke(defaultNotConnectedMessage));
            Debug.LogError("Cannot toggle streaming, not connected to OBS!");
            return;
        }

        try
        {
            var streamingStatus = obsWebSocket.GetStreamStatus();
            if (streamingStatus.IsActive)
            {
                obsWebSocket.StopStream();
            }
            else
            {
                obsWebSocket.StartStream();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error toggling streaming: " + e.Message);
        }
    }
    #endregion
}
