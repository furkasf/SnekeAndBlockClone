using UnityEngine;

namespace MyInput
{
    public class InputManager : MonoBehaviour
    {
        [Header("Data")] public InputData Data;

        [SerializeField] private bool isReadyForTouch, isFirstTimeTouchTaken;
        [SerializeField] private FloatingJoystick floatingJoystick;

        private bool _isTouching;
        private Vector3 _moveVector;

        private void Awake()
        {
            Data = GetInputData();
        }

        private InputData GetInputData() => Resources.Load<InputData>("Data/InputData");

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.onEnableInput += OnEnableInput;
            InputSignals.onDisableInput += OnDisableInput;

            InputSignals.onPlay += OnPlay;
            InputSignals.onPause += OnReset;
        }

        private void UnsubscribeEvents()
        {
            InputSignals.onEnableInput -= OnEnableInput;
            InputSignals.onDisableInput -= OnDisableInput;
            InputSignals.onPlay -= OnPlay;
            InputSignals.onPause -= OnReset;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void Update()
        {
            if (!isReadyForTouch) return;
            if (Input.GetMouseButtonUp(0))
            {
                _isTouching = false;
                InputSignals.onInputReleased?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                _isTouching = true;
                InputSignals.onInputTaken?.Invoke();
                if (!isFirstTimeTouchTaken)
                {
                    isFirstTimeTouchTaken = true;
                    InputSignals.onFirstTimeTouchTaken?.Invoke();
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_isTouching)
                {
                    InputSignals.onInputDragged?.Invoke(new InputParams(new Vector3(floatingJoystick.Horizontal, 0, floatingJoystick.Vertical)));
                }
            }
        }

        private void OnEnableInput()
        {
            isReadyForTouch = true;
        }

        private void OnDisableInput() => isReadyForTouch = false;

        private void OnPlay()
        {
            isReadyForTouch = true;
        }

        private void OnReset()
        {
            _isTouching = false;
            isReadyForTouch = false;
            isFirstTimeTouchTaken = false;
        }
    }
}