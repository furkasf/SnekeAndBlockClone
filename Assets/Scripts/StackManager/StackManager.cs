using Assets.Scripts;
using CameraSystem;
using Level;
using MyInput;
using Snake;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StackManager
{
    public class StackManager : MonoBehaviour
    {
        [SerializeField] private List<AbstractSnake> abstractSnakes;
        [SerializeField] private TMP_Text SneakNumber;
        [SerializeField] private bool _iReadyToPlay;

        private StackLerpMoveCommand _lerpStackCommand;
        private LerpData _lerpData;

        private void Start()
        {
            _iReadyToPlay = false; //temp
            Init();
        }

        private void GetLerpData() => _lerpData = Resources.Load<LerpData>("Data/LerpData");

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            InputSignals.onInputDragged += OnUpdateInputValue;
            InputSignals.onInputReleased += OnInputUpdateStop;

            StackSignals.onAddStack += OnAddStack;
            StackSignals.onRemoveStack += OnRemoveStack;
            StackSignals.onInPowerStack += OnInPowerStack;
            StackSignals.onReadyToPlay += OnReadyToPlay;
        }

        private void UnSubscribe()
        {
            InputSignals.onInputDragged -= OnUpdateInputValue;
            InputSignals.onInputReleased -= OnInputUpdateStop;

            StackSignals.onAddStack -= OnAddStack;
            StackSignals.onRemoveStack -= OnRemoveStack;
            StackSignals.onInPowerStack -= OnInPowerStack;
            StackSignals.onReadyToPlay -= OnReadyToPlay;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void FixedUpdate()
        {
            if (!_iReadyToPlay) return;
            _lerpStackCommand.Execute();
        }

        private void OnReadyToPlay(bool play) => _iReadyToPlay = play;

        private SnakePowerState OnInPowerStack() => abstractSnakes[0].PowerState;

        public void OnUpdateInputValue(InputParams inputParam)
        {
            _lerpData.LerpMoveDirection = inputParam.movementVector;
        }

        public void OnInputUpdateStop() => _lerpData.LerpMoveDirection = Vector3.zero;

        private void OnAddStack(AbstractSnake snake)
        {
            snake.transform.position = abstractSnakes[0].transform.position;
            abstractSnakes.Add(snake);
            GradiantStack(1f, (float)abstractSnakes.Count);
            abstractSnakes.TrimExcess();
            ReplaceSnakeText();
        }

        [ContextMenu(nameof(OnRemoveStack))]
        private void OnRemoveStack()
        {
            abstractSnakes.RemoveAt(0);
            abstractSnakes.TrimExcess();
            
            if (abstractSnakes.Count == 0)
            {

                SneakNumber.gameObject.SetActive(false);
                LevelSignals.onClearLevel();
                _iReadyToPlay = false;
                UISignals.onGameRestart();
                return;

            }

            abstractSnakes[0].tag = "First";

            GradiantStack(1f, (float)abstractSnakes.Count);
            abstractSnakes.TrimExcess();
            CameraSiganls.onGetTargetPos((abstractSnakes.Count > 0) ? abstractSnakes[1].transform : abstractSnakes[0].transform);

          
            ReplaceSnakeText();
        }

        private void GradiantStack(float min, float max)
        {
            if (OnInPowerStack() == SnakePowerState.Power) return;
            foreach (AbstractSnake snake in abstractSnakes)
            {
                snake.GradiantMaterial(min, max);
            }
        }

        private void ReplaceSnakeText()
        {
            SneakNumber.text = abstractSnakes.Count.ToString();
            SneakNumber.transform.parent = abstractSnakes[0].transform;
            SneakNumber.transform.localPosition = Vector3.forward;
        }

        private void Init()
        {
            GetLerpData();
            SneakNumber.gameObject.SetActive(true);
            GradiantStack(0f, (float)abstractSnakes.Count);
            _lerpStackCommand = new StackLerpMoveCommand(ref abstractSnakes, ref _lerpData);
            abstractSnakes[0].tag = "First";
            CameraSiganls.onGetTargetPos((abstractSnakes.Count > 0)? abstractSnakes[1].transform: abstractSnakes[0].transform);
            ReplaceSnakeText();
        }
    }
}