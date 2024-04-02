#if UNITY_PHYSICS

using UnityEngine;

namespace Flexalon.Templates
{
    // Clears the ConstraintPicker's selected field when this object is clicked.
    [DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Constraint Picker Deselect")]
    public class ConstraintPickerDeselect : MonoBehaviour
    {
        void Awake()
        {
            var interactable = GetComponent<FlexalonInteractable>();
            interactable.Clicked.AddListener((_) => {
                ConstraintPicker.ClearSelected();
            });
        }
    }
}

#endif