#if UNITY_PHYSICS

using System.Collections.Generic;
using UnityEngine;

namespace Flexalon.Templates
{
    // When the user clicks on an object with this component, it becomes selected.
    // If the user then clicks on an object with a ConstraintTarget, then
    // this component will search for a FlexalonConstraint component and assign its
    // target to the ConstraintTarget's gameObject.
    [DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Constraint Picker")]
    public class ConstraintPicker : MonoBehaviour
    {
        // Swap to this material to when the object is selected.
        [SerializeField]
        private Color _selectedColor;
        public Color SelectedColor
        {
            get => _selectedColor;
            set => _selectedColor = value;
        }

        // The currently selected ConstraintPicker.
        private static ConstraintPicker _selected;
        public static ConstraintPicker Selected => _selected;

        private FlexalonConstraint _constraint;
        private Dictionary<TemplateDynamicMaterial, Color> _originalColors = new Dictionary<TemplateDynamicMaterial, Color>();

        void Awake()
        {
            // Find the nearest constraint.
            _constraint = GetComponentInParent<FlexalonConstraint>();

            // When this object is clicked, assign it to be selected.
            var interactable = GetComponent<FlexalonInteractable>();
            interactable.Clicked.AddListener((_) => {
                SetSelected(this);
            });
        }

        public void SetTarget(ConstraintTarget target)
        {
            _constraint.Target = target.gameObject;
        }

        public static void SetSelected(ConstraintPicker picker)
        {
            if (_selected != picker)
            {
                // Put back the original materials on the previously selected object.
                _selected?.UpdateColor(false);

                _selected = picker;

                // Assign the selected material to the new selected object.
                _selected?.UpdateColor(true);
            }
        }

        public static void ClearSelected()
        {
            SetSelected(null);
        }

        public void UpdateColor(bool selected)
        {
            var materials = _constraint.GetComponentsInChildren<TemplateDynamicMaterial>();
            foreach (var tdm in materials)
            {
                if (selected)
                {
                    _originalColors[tdm] = tdm.Color;
                }

                tdm.SetColor(selected ? _selectedColor : _originalColors[tdm]);
            }
        }
    }
}

#endif