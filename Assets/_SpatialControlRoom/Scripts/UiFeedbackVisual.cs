using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UiFeedbackVisual : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private ParticleSystem particlesSystem;

    [SerializeField] private Transform headPosition; //Center Eye Anchor
    private float fadeInDuration = 0.1f; 
    private float displayDuration = 0.2f; 
    private float fadeOutDuration = 0.1f; 

    private Vector3 positionOffest = new Vector3(0, 0.15f, 0);

    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        particlesSystem = GetComponentInChildren<ParticleSystem>();
        gameObject.SetActive(false);
    }

    public void TriggerVisualFeedback(string textToShow, Transform spawnLocation)
    {
        transform.position = spawnLocation.position + positionOffest;
        transform.rotation = Quaternion.LookRotation(transform.position - headPosition.position);


        //gameObject.transform.rotation = spawnLocation.rotation;
        gameObject.SetActive(true);
        textMeshPro.text = textToShow;
        particlesSystem.Play();
        StartCoroutine(FeedbackSequence());
    }

    private IEnumerator FeedbackSequence()
    {
        
        yield return StartCoroutine(FadeTextToFullAlpha(fadeInDuration, textMeshPro));
        
        yield return new WaitForSeconds(displayDuration);
        
        yield return StartCoroutine(FadeTextToZeroAlpha(fadeOutDuration, textMeshPro));
        
        gameObject.SetActive(false);
    }

    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
