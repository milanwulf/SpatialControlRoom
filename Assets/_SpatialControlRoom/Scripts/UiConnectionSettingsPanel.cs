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

        //Refresh available NDI feeds on panel opening
        //ATTENTION: this gameobject need to be set inactive in editor otherwise the default NDI Source will be the NDIReceiver source set in editor
        ndiManager.GetAvailableNdiFeeds();
        UpdateNdiFeedDropdowns();
        AddNdiDropdownListeners();
    }

    private void OnDisable()
    {
        closeBtn.onClick.RemoveListener(ClosePanel);
        wsAddressInputField.onSelect.RemoveListener(AddressFieldSelected);
        wsPortInputField.onSelect.RemoveListener(PortFieldSelected);
        wsPasswordInputField.onSelect.RemoveListener(PasswordFieldSelected);
        keyboardSpawner.DestroyKeyboard();

        RemoveNdiDropdownListeners();
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

    private void UpdateNdiFeedDropdowns()
    {
        List<string> sourceNames = ndiManager.GetNdiSourceNames();

        for (int i = 0; i < ndiFeedDropdown.Count && i < 3; i++)
        {
            ndiFeedDropdown[i].ClearOptions();
            ndiFeedDropdown[i].AddOptions(sourceNames);

            // Get current NDI source name for this dropdown
            string currentSource = ndiManager.GetCurrentNdiSourceName(i + 1);

            if (!string.IsNullOrEmpty(currentSource))
            {
                // Find index of current NDI source name in the list
                int currentIndex = sourceNames.FindIndex(name => name == currentSource);
                // Set dropdown value to current NDI source name index
                if (currentIndex != -1)
                {
                    ndiFeedDropdown[i].value = currentIndex;
                }
            }
        }
    }
    private void AddNdiDropdownListeners()
    {
        for (int i = 0; i < ndiFeedDropdown.Count; i++)
        {
            // Local copy of i required for lambda expression to avoid closure pitfall
            int receiverId = i + 1;
            ndiFeedDropdown[i].onValueChanged.AddListener((value) =>
            {
                // Selected index is passed to NdiManager
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
}
