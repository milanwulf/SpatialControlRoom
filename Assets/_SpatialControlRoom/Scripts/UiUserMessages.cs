using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiUserMessages : MonoBehaviour
{
    [SerializeField] private OBSWebSocketManager obsWebsocketManager;

    private TextMeshProUGUI textMeshPro;
    //private ParticleSystem particlesSystem;

    private float fadeInDuration = 0.3f; 
    private float displayDuration = 3f; 
    private float fadeOutDuration = 0.3f;

    //Message Queue
    private Queue<string> messageQueue = new Queue<string>();
    private bool isDisplayingMessage = false;

    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        //particlesSystem = GetComponentInChildren<ParticleSystem>();
        textMeshPro.text = "";
    }

    private void OnEnable()
    {
        obsWebsocketManager.WsMessage += TriggerVisualFeedback;
    }

    private void OnDisable()
    {
        obsWebsocketManager.WsMessage -= TriggerVisualFeedback;
    }

    public void TriggerVisualFeedback(string textToShow)
    {
        messageQueue.Enqueue(textToShow);
        if (!isDisplayingMessage)
        {
            StartCoroutine(DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        isDisplayingMessage = true;
        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();
            textMeshPro.text = message;
            //particlesSystem.Play();

            yield return StartCoroutine(FadeTextToFullAlpha(fadeInDuration, textMeshPro));
            yield return new WaitForSeconds(displayDuration);
            yield return StartCoroutine(FadeTextToZeroAlpha(fadeOutDuration, textMeshPro));
        }
        isDisplayingMessage = false;
    }


    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, Mathf.Min(i.color.a + (Time.deltaTime / t), 1.0f));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, Mathf.Max(i.color.a - (Time.deltaTime / t), 0.0f));
            yield return null;
        }
    }

}
