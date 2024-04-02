#if UNITY_PHYSICS

using UnityEngine.EventSystems;
using UnityEngine;

namespace Flexalon.Templates
{
    // Camera controls for the templates scene. Allows the user to
    // move and rotate the camera, and click on templates to jump around.
    [AddComponentMenu("Flexalon Templates/Template Navigation")]
    public class TemplateNavigation : MonoBehaviour
    {
        // How fast the camera should move towards the target.
        [SerializeField]
        private float _interpolationSpeed = 10;
        public float InterpolationSpeed
        {
            get => _interpolationSpeed;
            set => _interpolationSpeed = value;
        }

        // How far the camera should be from a template when clicking on a template.
        [SerializeField]
        private float _focusDistance = 10;
        public float FocusDistance
        {
            get => _focusDistance;
            set => _focusDistance = value;
        }

        // How fast the camera should rotate with the mouse.
        [SerializeField]
        private float _rotateSpeed = 100;
        public float RotateSpeed
        {
            get => _rotateSpeed;
            set => _rotateSpeed = value;
        }

        // How fast the camera should zoom with scroll wheel.
        [SerializeField]
        private float _zoomSpeed = 100;
        public float ZoomSpeed
        {
            get => _zoomSpeed;
            set => _zoomSpeed = value;
        }

        // How fast the camera should move with WASD or arrows.
        [SerializeField]
        private float _moveSpeed = 10;
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        private float _xRotationStart;
        private float _yRotationStart;
        private float _zoomStart;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private Camera _camera;
        private Vector3 _mouseClickPosition;
        private Vector3 _mousePosition;
        private float _xRotation;
        private float _yRotation;
        private float _zoom;
        private Transform _arm;
        private bool _rotating;

        void Start()
        {
            _camera = Camera.main;
            _camera.transform.rotation = Quaternion.LookRotation(-_camera.transform.position, Vector3.up);
            _targetPosition = Vector3.zero;
            _zoom = _zoomStart = _camera.transform.position.magnitude;

            var armGO = new GameObject("Camera Arm");
            armGO.transform.rotation = _camera.transform.rotation;
            _camera.transform.SetParent(armGO.transform);
            _arm = armGO.transform;

            var euler = _camera.transform.rotation.eulerAngles;
            _xRotation = _xRotationStart = euler.x;
            _yRotation = _yRotationStart = euler.y;

            #if UNITY_2023_1_OR_NEWER
                var interactables = GameObject.FindObjectsByType<FlexalonInteractable>(FindObjectsSortMode.None);
            #else
                var interactables = GameObject.FindObjectsOfType<FlexalonInteractable>();
            #endif
            foreach (var interactable in interactables)
            {
                if (interactable.name.StartsWith("Floor"))
                {
                    interactable.Clicked.AddListener((i) => {
                        _targetPosition = i.transform.position;
                        _zoom = _focusDistance;
                    });
                }
            }
        }

        void Update()
        {
            bool dragging = (FlexalonInteractable.SelectedObject?.State == FlexalonInteractable.InteractableState.Dragging) || (FlexalonInteractable.HoveredObject?.Draggable ?? false);
            if (dragging || EventSystem.current.currentSelectedGameObject != null)
            {
                _rotating = false;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _mousePosition = Input.mousePosition;
                _mouseClickPosition = _mousePosition;
                _rotating = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _rotating = false;
            }

            if (Input.GetMouseButton(0))
            {
                if (_rotating)
                {
                    var delta = Input.mousePosition - _mousePosition;
                    _mousePosition = Input.mousePosition;
                    _xRotation -= delta.y * _rotateSpeed * Time.deltaTime;
                    _yRotation += delta.x * _rotateSpeed * Time.deltaTime;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _targetPosition = Vector3.zero;
                _xRotation = _xRotationStart;
                _yRotation = _yRotationStart;
                _zoom = _zoomStart;
            }

            var fwd = Camera.main.transform.forward;
            fwd.y = 0;
            fwd.Normalize();

            var right = Camera.main.transform.right;
            right.y= 0;
            right.Normalize();

            var moveSpeedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _targetPosition += fwd * _moveSpeed * moveSpeedMultiplier * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _targetPosition += -right * _moveSpeed * moveSpeedMultiplier * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _targetPosition += -fwd * _moveSpeed * moveSpeedMultiplier * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _targetPosition += right * _moveSpeed * moveSpeedMultiplier * Time.deltaTime;
            }

            _zoom -= Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
            _targetRotation = Quaternion.Euler(_xRotation, _yRotation, 0);

            _arm.position = Vector3.Lerp(_arm.position, _targetPosition, Time.deltaTime * _interpolationSpeed);
            _arm.rotation = Quaternion.Lerp(_arm.rotation, _targetRotation, Time.deltaTime * _interpolationSpeed);
            _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, Vector3.back * _zoom, Time.deltaTime * _interpolationSpeed);
        }
    }
}

#endif