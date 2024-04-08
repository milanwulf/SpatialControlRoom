using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication; // important for ObsDisconnectionInfo
using OBSWebsocketDotNet.Types;
using OBSWebsocketDotNet.Types.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OBSWebSocketManager : MonoBehaviour
{
    private OBSWebsocket obsWebSocket = new OBSWebsocket();
    [SerializeField] private string serverAdress = "localhost";
    [SerializeField] private int serverPort = 4444;
    [SerializeField] private string serverPassword = "";
    [SerializeField] private UiFeedInstanceManger uiFeedInstanceManger;

    private Queue<Action> actionsToExectuteOnMainThread = new Queue<Action>();

    private void Awake()
    {
        actionsToExectuteOnMainThread = new Queue<Action>();
    }

    void Start()
    {
        ConnectToServer(serverAdress, serverPort, serverPassword);
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
                Debug.LogWarning("Versuchte, eine null-Action auszuführen.");
            }
        }
    }

    public void ConnectToServer(string serverAdress, int serverPort, string serverPassword)
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
        obsWebSocket.RecordStateChanged += OBSRecordStateChanged;
        SetInitRecordingState();
    }

    private void OnDisconnected(object sender, ObsDisconnectionInfo e)
    {
        Debug.Log($"Disconnected from OBS WebSocket Server. Reason: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}");
        obsWebSocket.CurrentProgramSceneChanged -= CurrentProgramSceneChanged;
        obsWebSocket.CurrentPreviewSceneChanged -= CurrentPreviewSceneChanged;
        obsWebSocket.RecordStateChanged -= OBSRecordStateChanged;
    }

    void OnDestroy()
    {
        if (obsWebSocket.IsConnected)
            obsWebSocket.Disconnect();
    }

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

    public void TriggerStudioModeTransition()
    {
        if (!obsWebSocket.IsConnected)
        {
            Debug.LogError("Cant trigger Studio Mode Transition, not connected to OBS!");
            return;
        }

        if(!obsWebSocket.GetStudioModeEnabled())
        {
            Debug.LogError("Studio Mode is not enabled");
            return;
        }

        try
        {
            obsWebSocket.TriggerStudioModeTransition();
        }
        catch (Exception e)
        {
            Debug.LogError("Error triggering Studio Mode Transition: " + e.Message);
        }
    }

    /// Recording Logic
    public bool IsRecording { get; private set; }
    public event Action<bool> RecordingState;

    private void SetInitRecordingState()
    {
        var recordingStatus = obsWebSocket.GetRecordStatus();
        IsRecording = recordingStatus.IsRecording;
        Debug.Log("Initial Recording State: " + IsRecording);
        RecordingState?.Invoke(IsRecording);
    }

    private void OBSRecordStateChanged(object sender, RecordStateChangedEventArgs e)
    {
        string unfilteredState = e.OutputState.StateStr; //needed to distinguish between OBS_WEBSOCKET_OUTPUT_STARTING / OBS_WEBSOCKET_OUTPUT_STARTED and OBS_WEBSOCKET_STOPPING / OBS_WEBSOCKET_OUTPUT_STOPPED / OBS_WEBSOCKET_OUTPUT_PAUSED

        if(unfilteredState == "OBS_WEBSOCKET_OUTPUT_STARTED")
        {
            IsRecording = true;
            RecordingState?.Invoke(IsRecording);
            Debug.Log("Recording State Changed: " + IsRecording);
        }

        else if (unfilteredState == "OBS_WEBSOCKET_OUTPUT_STOPPED")
        {
            IsRecording = false;
            RecordingState?.Invoke(IsRecording);
            Debug.Log("Recording State Changed: " + IsRecording);
        }
    }

    public void ToggleRecording()
    {
        if (!obsWebSocket.IsConnected)
        {
            Debug.LogError("Cannot toggle recording, not connected to OBS!");
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

    /*
    public string GetRecordingTimeCode()
    {
        var timecode = "00:00:00";
        if (!obsWebSocket.IsConnected)
        {
            Debug.LogError("Cannot get recording time code, not connected to OBS!");
            timecode = "00:00:00";
            return timecode;
        }

        try
        {
            var recordingStatus = obsWebSocket.GetRecordStatus();
            timecode = recordingStatus.RecordTimecode;
            return timecode.Split('.')[0];
        }

        catch (Exception e)
        {
            Debug.LogError("Error getting recording time code: " + e.Message);
            timecode = "00:00:00";
            return timecode;
        }
    }
    */
}
