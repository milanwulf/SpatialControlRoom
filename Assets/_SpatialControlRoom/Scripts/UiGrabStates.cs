using UnityEngine;
using Oculus.Interaction;

// Stelle sicher, dass dieses Script auf das GameObject mit dem zu ändernden Material gelegt wird.
public class UiGrabStates : MonoBehaviour
{
    // Referenz auf das Script mit den Events, das auf demselben oder einem anderen GameObject liegen kann.
    [SerializeField] private PointableUnityEventWrapper eventWrapper;

    // Definiere die Farben, die verwendet werden sollen.
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color selectColor = Color.green;
    private Color defaultColor;
    private Material material;

    private void Awake()
    {
        // Versuche, das Renderer-Component zu erhalten und speichere die Standardfarbe des Materials.
        material = GetComponent<Renderer>().material;
        defaultColor = material.color;
    }

    private void OnEnable()
    {
        // Abonniere die Events.
        eventWrapper.WhenHover.AddListener(OnHover);
        eventWrapper.WhenUnhover.AddListener(OnUnhover);
        eventWrapper.WhenSelect.AddListener(OnSelect);
        eventWrapper.WhenUnselect.AddListener(OnUnselect);
    }

    private void OnDisable()
    {
        // Hebe das Abonnement der Events auf.
        eventWrapper.WhenHover.RemoveListener(OnHover);
        eventWrapper.WhenUnhover.RemoveListener(OnUnhover);
        eventWrapper.WhenSelect.RemoveListener(OnSelect);
        eventWrapper.WhenUnselect.RemoveListener(OnUnselect);
    }

    // Event-Handler-Funktionen.
    private void OnHover(PointerEvent evt)
    {
        material.color = hoverColor;
    }

    private void OnUnhover(PointerEvent evt)
    {
        // Zurück zur Standardfarbe, wenn das Objekt nicht ausgewählt ist.
        material.color = defaultColor;
    }

    private void OnSelect(PointerEvent evt)
    {
        material.color = selectColor;
    }

    private void OnUnselect(PointerEvent evt)
    {
        // Setze die Farbe zurück, wenn das Objekt nicht mehr ausgewählt ist.
        material.color = defaultColor;
    }
}
