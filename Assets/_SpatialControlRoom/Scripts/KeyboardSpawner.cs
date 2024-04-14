using System.Collections;
using Keyboard;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject keyboardPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform headPosition; // Center Eye Anchor
    [SerializeField] private string closeKeyboardButtonName = "CloseKeyboardButton";

    private GameObject currentKeyboard;
    private KeyboardManager keyboardManager;

    private void Awake()
    {
        if(headPosition == null)
        {
            headPosition = GameObject.Find("/OVRCameraRigInteraction/OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
        }
    }

    public void InstantiateKeyboard(TMP_InputField activeInputField)
    {
        if (currentKeyboard != null)
        {
            if (keyboardManager != null && keyboardManager.CurrentInputField == activeInputField)
            {
                // Das Keyboard ist bereits für das aktuelle Inputfeld geöffnet.
                return;
            }
            else
            {
                StartCoroutine(DestroyKeyboardThenInstantiate(activeInputField));
                return;
            }
        }

        CreateKeyboard(activeInputField);
    }

    private IEnumerator DestroyKeyboardThenInstantiate(TMP_InputField activeInputField)
    {
        yield return DestroyKeyboardAfterDelay();
        CreateKeyboard(activeInputField);
    }

    private void CreateKeyboard(TMP_InputField activeInputField)
    {
        currentKeyboard = Instantiate(keyboardPrefab, spawnLocation.position, Quaternion.LookRotation(transform.position - headPosition.position));
        keyboardManager = currentKeyboard.GetComponent<KeyboardManager>();

        if (keyboardManager != null)
        {
            keyboardManager.SetInputField(activeInputField);
            Button closeKeyboardBtn = currentKeyboard.transform.Find(closeKeyboardButtonName)?.GetComponent<Button>();
            if (closeKeyboardBtn != null)
            {
                closeKeyboardBtn.onClick.AddListener(() => StartCoroutine(DestroyKeyboardAfterDelay()));
            }
            else
            {
                Debug.LogError($"CloseButton with name {closeKeyboardButtonName} not found in {keyboardPrefab}!");
            }
        }
        else
        {
            Debug.LogError($"KeyboardManager Component not found in {keyboardPrefab}!");
        }
    }

    public void DestroyKeyboard()
    {
        StartCoroutine(DestroyKeyboardAfterDelay());
    }

    // Die Coroutine, die tatsächlich für das Zerstören des Keyboards verantwortlich ist.
    private IEnumerator DestroyKeyboardAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Kurze Verzögerung

        if (currentKeyboard != null)
        {
            Button closeKeyboardBtn = currentKeyboard.transform.Find(closeKeyboardButtonName)?.GetComponent<Button>();
            if (closeKeyboardBtn != null)
            {
                closeKeyboardBtn.onClick.RemoveListener(DestroyKeyboard);
            }
            Destroy(currentKeyboard);
            currentKeyboard = null;
            keyboardManager = null;

            // Fokus entfernen, um zu verhindern, dass das Keyboard unmittelbar neu erscheint.
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
