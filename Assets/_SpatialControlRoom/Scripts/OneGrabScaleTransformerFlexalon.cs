using System;
using UnityEngine;
using Oculus.Interaction.Samples;
using Flexalon; // Stelle sicher, dass du den Flexalon-Namespace hinzufügst

namespace Oculus.Interaction.Samples
{
    public class OneGrabScaleTransformerFlexalon : MonoBehaviour, ITransformer
    {
        [Serializable]
        public class OneGrabScaleConstraints
        {
            public bool IgnoreFixedAxes;
            public bool ConstrainXYAspectRatio;
            public FloatConstraint MinX;
            public FloatConstraint MaxX;
            public FloatConstraint MinY;
            public FloatConstraint MaxY;
            public FloatConstraint MinZ;
            public FloatConstraint MaxZ;
        }

        [SerializeField, Tooltip("Constraints for allowable values on different axes")]
        private OneGrabScaleConstraints _constraints = new OneGrabScaleConstraints()
        {
            IgnoreFixedAxes = false,
            ConstrainXYAspectRatio = false,
            MinX = new FloatConstraint(),
            MaxX = new FloatConstraint(),
            MinY = new FloatConstraint(),
            MaxY = new FloatConstraint(),
            MinZ = new FloatConstraint(),
            MaxZ = new FloatConstraint()
        };

        private Vector3 _initialLocalScale;
        private Vector3 _initialLocalPosition;
        private IGrabbable _grabbable;
        [SerializeField] private FlexalonObject _flexalonObject; // Referenz auf das Flexalon-Objekt

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            //_flexalonObject = GetComponent<FlexalonObject>(); // Versuche, die Flexalon-Komponente zu erhalten
            if (_flexalonObject == null)
            {
                Debug.LogError("Kein FlexalonObject gefunden am Objekt.");
            }
        }

        public void BeginTransform()
        {
            var grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;
            _initialLocalPosition = targetTransform.InverseTransformPointUnscaled(grabPoint.position);
            _initialLocalScale = _flexalonObject != null ? _flexalonObject.Scale : Vector3.one; // Verwende Flexalon-Skala, falls verfügbar
        }

        public void UpdateTransform()
        {
            if (_flexalonObject == null) return; // Nichts tun, wenn kein Flexalon-Objekt vorhanden ist

            var grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;
            var localPosition = targetTransform.InverseTransformPointUnscaled(grabPoint.position);

            float newLocalScaleX = _initialLocalScale.x * localPosition.x / _initialLocalPosition.x;
            float newLocalScaleY = _initialLocalScale.y * localPosition.y / _initialLocalPosition.y;
            float newLocalScaleZ = _initialLocalScale.z * localPosition.z / _initialLocalPosition.z;

            if (_constraints.MinX.Constrain)
            {
                newLocalScaleX = Mathf.Max(_constraints.MinX.Value, newLocalScaleX);
            }
            if (_constraints.MinY.Constrain)
            {
                newLocalScaleY = Mathf.Max(_constraints.MinY.Value, newLocalScaleY);
            }
            if (_constraints.MinZ.Constrain)
            {
                newLocalScaleZ = Mathf.Max(_constraints.MinZ.Value, newLocalScaleZ);
            }
            if (_constraints.MaxX.Constrain)
            {
                newLocalScaleX = Mathf.Min(_constraints.MaxX.Value, newLocalScaleX);
            }
            if (_constraints.MaxY.Constrain)
            {
                newLocalScaleY = Mathf.Min(_constraints.MaxY.Value, newLocalScaleY);
            }
            if (_constraints.MaxZ.Constrain)
            {
                newLocalScaleZ = Mathf.Min(_constraints.MaxZ.Value, newLocalScaleZ);
            }

            if (_constraints.IgnoreFixedAxes)
            {
                if (_constraints.MinX.Constrain && _constraints.MaxX.Constrain && _constraints.MinX.Value == _constraints.MaxX.Value)
                {
                    newLocalScaleX = targetTransform.localScale.x;
                }
                if (_constraints.MinY.Constrain && _constraints.MaxY.Constrain && _constraints.MinY.Value == _constraints.MaxY.Value)
                {
                    newLocalScaleY = targetTransform.localScale.y;
                }
                if (_constraints.MinZ.Constrain && _constraints.MaxZ.Constrain && _constraints.MinZ.Value == _constraints.MaxZ.Value)
                {
                    newLocalScaleZ = targetTransform.localScale.z;
                }
            }

            if (_constraints.ConstrainXYAspectRatio)
            {
                if (newLocalScaleX / newLocalScaleY < _initialLocalScale.x / _initialLocalScale.y)
                {
                    newLocalScaleY = newLocalScaleX * _initialLocalScale.y / _initialLocalScale.x;
                }
                else
                {
                    newLocalScaleX = newLocalScaleY * _initialLocalScale.x / _initialLocalScale.y;
                }
            }

            Vector3 newScale = new Vector3(newLocalScaleX, newLocalScaleY, newLocalScaleZ);
            _flexalonObject.Scale = newScale; // Aktualisiere die Flexalon-Skala
        }

        public void EndTransform()
        {
            // Hier könnten Aufräumarbeiten oder Abschlusslogik platziert werden
        }
    }
}
