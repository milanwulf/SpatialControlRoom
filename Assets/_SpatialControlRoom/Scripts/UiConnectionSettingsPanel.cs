using Klak.Ndi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiConnectionSettingsPanel : MonoBehaviour
{

    [SerializeField] private OBSWebSocketManager obsWebsocketManager;
    [SerializeField] private KeyboardSpawner keyboardSpawner;
    [SerializeField] private NdiManager ndiManager;

    [Header("Websocket Input Fields")]
    [SerializeField] private TMP_InputField wsAddressInputField;
    [SerializeField] private TMP_InputField wsPortInputField;
    [SerializeField] private TMP_InputField wsPasswordInputField;

    [Header("NDI Drowdowns")]
    [SerializeField] private List<TMP_Dropdown> ndiFeedDropdown;

    [Header("Connect Button")]
    [SerializeField] private GameObject connectBtnObject;
    private Button connectBtn;
    private GameObject connectBtnloadingSpinner;
    private GameObject connectBtnIcon;

    [Header("Close Button")]
    [SerializeField] private Button closeBtn;

    private void Awake()
    {
        InitializeConnectButton();
        gameObject.SetActive(false); //this gameobject need to be set inactive on Startup otherwise the default NDI Source will be the NDIReceiver source set in editor
    }

    private void InitializeConnectButton()
    {
        connectBtn = connectBtnObject.GetComponent<Button>();
        connectBtnIcon = connectBtnObject.transform.Find("Button Front/Icon").gameObject;
        connectBtnloadingSpinner = connectBtnObject.transform.Find("Button Front/LoadingSpinner").gameObject;
        ShowConnectButtonLoadingSpinner(false);
    }

    private void ShowConnectButtonLoadingSpinner(bool isLoading)
    {
        if(isLoading)
        {
            connectBtnIcon.SetActive(false);
            connectBtnloadingSpinner.SetActive(true);
        }
        else
        {
            connectBtnIcon.SetActive(true);
            connectBtnloadingSpinner.SetActive(false);
        }
    }

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        wsAddressInputField.onSelect.AddListener(AddressFieldSelected);
        wsPortInputField.onSelect.AddListener(PortFieldSelected);
        wsPasswordInputField.onSelect.AddListener(PasswordFieldSelected);

        //Refresh available NDI feeds on panel opening
        ndiManager.GetAvailableNdiFeeds();
        UpdateNdiFeedDropdowns();
        AddNdiDropdownListeners();

        //Websocket connection
        connectBtn.onClick.AddListener(ConnectToObs);
        LoadLastConnectionData();
        obsWebsocketManager.WsConnected += WebsocketConnectedEvent;

    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
        wsAddressInputField.onSelect.RemoveListener(AddressFieldSelected);
        wsPortInputField.onSelect.RemoveListener(PortFieldSelected);
        wsPasswordInputField.onSelect.RemoveListener(PasswordFieldSelected);
        keyboardSpawner.DestroyKeyboard();
        
        RemoveNdiDropdownListeners();

        connectBtn.onClick.RemoveListener(ConnectToObs);
        obsWebsocketManager.WsConnected -= WebsocketConnectedEvent;
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    #region Websocket Handling
    private void AddressFieldSelected(string s)
    {
        keyboardSpawner.InstantiateKeyboard(wsAddressInputField);
    }

    private void PortFieldSelected(string s)
    {
        keyboardSpawner.InstantiateKeyboard(wsPortInputField);
    }

    private void PasswordFieldSelected(string s)
    {
        keyboardSpawner.InstantiateKeyboard(wsPasswordInputField);
    }
    private void ConnectToObs()
    {
       if (obsWebsocketManager != null)
       {
            if(int.TryParse(wsPortInputField.text, out int port))
            {
                obsWebsocketManager.ConnectToServer(wsAddressInputField.text, int.Parse(wsPortInputField.text), wsPasswordInputField.text);
                //ShowConnectButtonLoadingSpinner(true);
                SaveCurrentConnectionData();
            }

            else
            {
                Debug.LogWarning("Port needs to be a number!");
            }
       }
    }

    private void SaveCurrentConnectionData()
    {
        PlayerPrefs.SetString("wsAddress", wsAddressInputField.text);
        PlayerPrefs.SetInt("wsPort", int.TryParse(wsPortInputField.text, out int port) ? port : 4455); //if parsing fails, set default port
        PlayerPrefs.SetString("wsPassword", wsPasswordInputField.text); //TODO: encrypt password
    }

    private void LoadLastConnectionData()
    {
        string wsAddress = PlayerPrefs.GetString("wsAddress");
        string wsPort = PlayerPrefs.GetInt("wsPort").ToString();
        string wsPassword = PlayerPrefs.GetString("wsPassword");
        if (!string.IsNullOrEmpty(wsAddress))
        {
            wsAddressInputField.text = wsAddress;
            wsPortInputField.text = wsPort;
            wsPasswordInputField.text = wsPassword;
        }
    }

    private void WebsocketConnectedEvent(bool wsConnected)
    {
        if(wsConnected)
        {
            //ShowConnectButtonLoadingSpinner(false);
        }

        else
        {
                
        }
    }
    #endregion

    #region NDI Handling
    private void UpdateNdiFeedDropdowns()
    {
        List<string> sourceNames = new List<string> { "None" };
        sourceNames.AddRange(ndiManager.GetNdiSourceNames());

        for (int i = 0; i < ndiFeedDropdown.Count; i++)
        {
            int receiverId = i + 1;
            ndiFeedDropdown[i].ClearOptions();
            ndiFeedDropdown[i].AddOptions(sourceNames);

            // Setze den Dropdown-Index basierend auf dem gespeicherten Wert
            int savedIndex = PlayerPrefs.GetInt($"Receiver{receiverId}Selection", 0); // Default auf "None"
            ndiFeedDropdown[i].value = savedIndex;
        }
    }

    private void AddNdiDropdownListeners()
    {
        for (int i = 0; i < ndiFeedDropdown.Count; i++)
        {
            int receiverId = i + 1;
            ndiFeedDropdown[i].onValueChanged.AddListener((value) =>
            {
                PlayerPrefs.SetInt($"Receiver{receiverId}Selection", value);
                PlayerPrefs.Save();

                // Set the source in the NdiManager, including disabling if "None" was selected
                ndiManager.SetNdiReceiverSource(receiverId, value);
            });
        }
    }

    private void RemoveNdiDropdownListeners()
    {
        for (int i = 0; i < ndiFeedDropdown.Count; i++)
        {
            ndiFeedDropdown[i].onValueChanged.RemoveAllListeners();
        }
    }
    #endregion
}
