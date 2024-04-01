using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Christina.UI
{
    public class UiToggleSwitch : MonoBehaviour
    {
        [Header("Slider setup")]
        [SerializeField, Range(0, 1f)]
        protected float sliderValue;
        public bool CurrentValue { get; private set; }

        private bool _previousValue;
        private Slider _slider;

        [Header("Animation")]
        [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;
        [SerializeField]
        private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine _animateSliderCoroutine;

        [Header("Events")]
        [SerializeField] private UnityEvent onToggleOn;
        [SerializeField] private UnityEvent onToggleOff;
        public UnityEvent OnToggleOn => onToggleOn;
        public UnityEvent OnToggleOff => onToggleOff;

        protected Action transitionEffect;

        protected virtual void OnValidate()
        {
            SetupToggleComponents();
            // Directly set the slider value here to update the slider in the editor.
            if (_slider != null) _slider.value = sliderValue;
        }

        private void SetupToggleComponents()
        {
            if (_slider != null) return;
            SetupSliderComponent();
        }

        private void SetupSliderComponent()
        {
            _slider = GetComponent<Slider>();
            if (_slider == null)
            {
                Debug.Log("No slider found!", this);
                return;
            }
            _slider.interactable = false;
            var sliderColors = _slider.colors;
            sliderColors.disabledColor = Color.white;
            _slider.colors = sliderColors;
            _slider.transition = Selectable.Transition.None;
        }

        protected virtual void Awake()
        {
            SetupSliderComponent();
        }

        public void Toggle()
        {
            SetStateAndStartAnimation(!CurrentValue);
        }

        public void ToggleByGroupManager(bool valueToSetTo)
        {
            SetStateAndStartAnimation(valueToSetTo);
        }

        private void SetStateAndStartAnimation(bool state)
        {
            _previousValue = CurrentValue;
            CurrentValue = state;

            if (_previousValue != CurrentValue)
            {
                if (CurrentValue)
                    onToggleOn?.Invoke();
                else
                    onToggleOff?.Invoke();
            }

            if (_animateSliderCoroutine != null)
                StopCoroutine(_animateSliderCoroutine);

            _animateSliderCoroutine = StartCoroutine(AnimateSlider());
        }

        public void SetToggleStateDirectly(bool state)
        {
            CurrentValue = state;

            if (_animateSliderCoroutine != null)
            {
                StopCoroutine(_animateSliderCoroutine);
            }

            _animateSliderCoroutine = StartCoroutine(AnimateSliderDirectly(state ? 1 : 0));
        }

        private IEnumerator AnimateSlider()
        {
            float startValue = _slider.value;
            float endValue = CurrentValue ? 1 : 0;

            float time = 0;
            if (animationDuration > 0)
            {
                while (time < animationDuration)
                {
                    time += Time.deltaTime;
                    float lerpFactor = slideEase.Evaluate(time / animationDuration);
                    _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);
                    transitionEffect?.Invoke();
                    yield return null;
                }
            }
            _slider.value = sliderValue = endValue;
        }

        private IEnumerator AnimateSliderDirectly(float targetValue)
        {
            float startValue = _slider.value;
            float time = 0;

            while (time < animationDuration)
            {
                time += Time.deltaTime;
                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                _slider.value = Mathf.Lerp(startValue, targetValue, lerpFactor);
                yield return null;
            }

            _slider.value = targetValue;

            sliderValue = targetValue;
        }

    }
}
