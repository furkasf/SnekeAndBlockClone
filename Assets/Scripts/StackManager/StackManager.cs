using Level;
using MyInput;
using Snake;
using System.Collections.Generic;
using UnityEngine;

namespace StackManager
{
    public class StackManager : MonoBehaviour
    {
        [SerializeField] private List<AbstractSnake> abstractSnakes;
        [SerializeField] private bool _iReadyToPlay;

        private StackLerpMoveCommand _lerpStackCommand;
        private LerpData _lerpData;

        private void Awake()
        {
            Init();
        }

        private void GetLerpData() => _lerpData = Resources.Load<LerpData>("Data/LerpData");

        private void FixedUpdate()
        {
            if (!_iReadyToPlay) return;
            _lerpStackCommand.Execute();
        }

        private void OnEnable()
        {
            Subscribe();
            CameraSiganls.onGetTargetPos(abstractSnakes[0].transform);
        }

        private void Subscribe()
        {
            InputSignals.onInputDragged += OnUpdateInputValue;
            InputSignals.onInputReleased += OnInputUpdateStop;

            StackSignals.onAddStack += OnAddStack;
            StackSignals.onRemoveStack += OnRemoveStack;
        }

        private void UnSubscribe()
        {
            InputSignals.onInputDragged -= OnUpdateInputValue;
            InputSignals.onInputReleased -= OnInputUpdateStop;

            StackSignals.onAddStack -= OnAddStack;
            StackSignals.onRemoveStack -= OnRemoveStack;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        public void OnUpdateInputValue(InputParams inputParam)
        {
            _lerpData.LerpMoveDirection = inputParam.movementVector;
        }

        public void OnInputUpdateStop() => _lerpData.LerpMoveDirection = Vector3.zero;

        private void OnAddStack(AbstractSnake snake)
        {
            snake.transform.position = abstractSnakes[0].transform.position;
            abstractSnakes.Add(snake);
        }

        private void OnRemoveStack()
        {
            abstractSnakes.RemoveAt(0);
            abstractSnakes.TrimExcess();
            if (abstractSnakes.Count == 0)
            {
                LevelSignals.onClearLevel();
                _iReadyToPlay = false;
                //open UI
                return;
            }
            abstractSnakes[0].tag = "First";
            CameraSiganls.onGetTargetPos(abstractSnakes[0].transform);
        }

        private void Init()
        {
            GetLerpData();
            _lerpStackCommand = new StackLerpMoveCommand(ref abstractSnakes, ref _lerpData);
            abstractSnakes[0].tag = "First";
        }
    }
}