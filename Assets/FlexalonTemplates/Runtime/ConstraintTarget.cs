#if UNITY_PHYSICS

using UnityEngine;

namespace Flexalon.Templates
{
    // See ConstraintPicker.
    [DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Constraint Target")]
    public class ConstraintTarget : MonoBehaviour
    {
        void Awake()
        {
            var interactable = GetComponent<FlexalonInteractable>();
            interactable.Clicked.AddListener((_) => {
                ConstraintPicker.Selected?.SetTarget(this);
            });

            // Clear the currently selected ConstraintPicker if this object starts being dragged.
            interactable.DragStart.AddListener((_) => {
                ConstraintPicker.ClearSelected();
            });
        }
    }
}

#endif