#if UNITY_PHYSICS

using UnityEngine;

namespace Flexalon.Templates
{
    // Assigns different material colors to an object based on the FlexalonInteractable state.
    [DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Interactable Style")]
    public class InteractableStyle : MonoBehaviour
    {
        [SerializeField]
        private Color _hoverColor;
        public Color HoverColor
        {
            get => _hoverColor;
            set => _hoverColor = value;
        }

        [SerializeField]
        private Color _selectColor;
        public Color SelectColor
        {
            get => _selectColor;
            set => _selectColor = value;
        }

        [SerializeField]
        private Color _dragColor;
        public Color DragColor
        {
            get => _dragColor;
            set => _dragColor = value;
        }

        private Color _originalColor;

        void Awake()
        {
            var material = GetComponent<TemplateDynamicMaterial>();

            var interactable = GetComponent<FlexalonInteractable>();
            interactable.HoverStart.AddListener(_ => {
                _originalColor = material.Color;
                material.SetColor(_hoverColor);
            });

            interactable.HoverEnd.AddListener(_ => {
                material.SetColor(_originalColor);
            });

            interactable.SelectStart.AddListener(_ => {
                material.SetColor(_selectColor);
            });

            interactable.SelectEnd.AddListener(_ => {
                material.SetColor(_hoverColor);
            });

            interactable.DragStart.AddListener(_ => {
                material.SetColor(_dragColor);
            });
        }
    }
}

#endif