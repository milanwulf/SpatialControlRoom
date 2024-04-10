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

    [Header("Websocket Input Fields")]
    [SerializeField] private TMP_InputField wsAddressInputField;
    [SerializeField] private TMP_InputField wsPortInputField;
    [SerializeField] private TMP_InputField wsPasswordInputField;

    [Header("NDI Drowdowns")]
    [SerializeField] private TMP_Dropdown ndiFeed1Dropdown;
    [SerializeField] private TMP_Dropdown ndiFeed2Dropdown;
    [SerializeField] private TMP_Dropdown ndiFeed3Dropdown;

    [Header("NDI Receiver")] //should be in a extra NDIManager script
    [SerializeField] private NdiReceiver ndiReceiver1;
    [SerializeField] private NdiReceiver ndiReceiver2;
    [SerializeField] private NdiReceiver ndiReceiver3;

    [Header("Connect Button")] //TODO: add connection message
    [SerializeField] private Button connectBtn;

    [Header("Close Button")]
    [SerializeField] private Button closeBtn;

    private void OnEnable()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        wsAddressInputField.onSelect.AddListener(AddressFieldSelected);
        wsPortInputField.onSelect.AddListener(PortFieldSelected);
        wsPasswordInputField.onSelect.AddListener(PasswordFieldSelected);
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
        wsAddressInputField.onSelect.RemoveListener(AddressFieldSelected);
        wsPortInputField.onSelect.RemoveListener(PortFieldSelected);
        wsPasswordInputField.onSelect.RemoveListener(PasswordFieldSelected);
        keyboardSpawner.DestroyKeyboard();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }

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
}
