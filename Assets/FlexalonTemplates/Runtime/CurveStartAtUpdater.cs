using UnityEngine;

namespace Flexalon.Templates
{
    // Updates a Curve's StartAt attribute at each frame to make objects animate along the curve.
    [ExecuteAlways, RequireComponent(typeof(FlexalonCurveLayout)), DisallowMultipleComponent, AddComponentMenu("Flexalon Templates/Curve Start At Updater")]
    public class CurveStartAtUpdater : MonoBehaviour
    {
        public enum WrapType
        {
            // Play animation once, then stop.
            OneShot,

            // Loop each time from the beginning.
            Repeat,

            // Play in reverse.
            PingPong
        }

        // What should happen when reaching the end of the curve?
        [SerializeField]
        private WrapType _wrap = WrapType.Repeat;
        public WrapType Wrap
        {
            get => _wrap;
            set { _wrap = value; }
        }

        // Time curve for easing effects.
        [SerializeField]
        private AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve Curve
        {
            get => _animationCurve;
            set { _animationCurve = value; }
        }

        // What value should StartAt begin at.
        [SerializeField]
        private float _startOffset = 0;
        public float StartOffset
        {
            get => _startOffset;
            set { _startOffset = value; }
        }

        // What value should StartAt end at, as an offset from the CurveLength.
        [SerializeField]
        private float _endOffset = 0;
        public float EndOffset
        {
            get => _endOffset;
            set { _endOffset = value; }
        }

        // Should this animation play in edit mode? Useful for testing different values.
        [SerializeField]
        private bool _runInEditMode = false;
        public bool RunInEditMode
        {
            get => _runInEditMode;
            set { _runInEditMode = value; }
        }

        // Tracks how much time has elapsed.
        [SerializeField]
        private float _animationTime = 0;
        public float AnimationTime
        {
            get => _animationTime;
            set { _animationTime = value; }
        }

        private FlexalonCurveLayout _curveLayout;
        private float _startTime;
        private float _lastTime;

        private void OnEnable()
        {
            _startTime = Time.time;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.update += EditorUpdate;
            }
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorApplication.update -= EditorUpdate;
            }
#endif
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                UpdateTime();
                UpdateCurveLayout();
            }
        }

        private void EditorUpdate()
        {
            if (_runInEditMode)
            {
                UpdateTime();
            }

            UpdateCurveLayout();
        }

        private void UpdateTime()
        {
            if (!Application.isPlaying && !_runInEditMode)
            {
                return;
            }

            _animationTime = Time.time - _startTime;
        }

        private void UpdateCurveLayout()
        {
            if (_curveLayout == null)
            {
                _curveLayout = GetComponent<FlexalonCurveLayout>();
            }

            if (_lastTime != _animationTime)
            {
                var time = _animationTime;
                var animationCurveTime = _animationCurve.keys[_animationCurve.length - 1].time - _animationCurve.keys[0].time;
                if (_wrap == WrapType.Repeat)
                {
                    time -= Mathf.Floor(time / animationCurveTime) * animationCurveTime;
                }
                else if (_wrap == WrapType.PingPong)
                {
                    bool pong = Mathf.Floor(time / animationCurveTime) % 2 == 1;
                    time -= Mathf.Floor(time / animationCurveTime) * animationCurveTime;
                    if (pong)
                    {
                        time = animationCurveTime - time;
                    }
                }

                var length = _curveLayout.CurveLength - _startOffset + _endOffset;
                _curveLayout.StartAt = _startOffset + _animationCurve.Evaluate(time) * length;
                _lastTime = _animationTime;
            }
        }
    }
}