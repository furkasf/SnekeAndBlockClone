using GameCore;
using GenericPoolSystem;
using GradientSystem;
using StackManager;
using UnityEngine;

namespace Snake
{
    public class Snake : AbstractSnake
    {
        public override Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
        public override SnakePowerState PowerState { get => _powerState; set => _powerState = value; }

        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private Material standartMaterial;
        [SerializeField] private Material rawinBowMatarial;
        [SerializeField] private SnakeState snakeState = SnakeState.UnStack;

        private Renderer _renderer;
        private SnakePowerState _powerState = SnakePowerState.Normal;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            StackSignals.onEnterPowerState += OnEnterPowerMode;
            AddSelfToStack();
        }

        private void OnDisable()
        {
            StackSignals.onEnterPowerState -= OnEnterPowerMode;
            if (this == null)
            {
                return;
            }
            SnakeSelfReset();
        }

        private void SnakeSelfReset()
        {
            if (snakeState == SnakeState.InStack)
            {
                tag = "Untagged";
                GameObject partical = PoolSignals.onGetObjectFormPool?.Invoke("SnakePartical");
                partical.transform.position = transform.position;
                StackSignals.onRemoveStack();
                _renderer.material = standartMaterial;
                snakeState = SnakeState.UnStack;
            }
        }

        private void AddSelfToStack()
        {
            if (snakeState == SnakeState.UnStack && CoreGameSignals.onGamePlay())
            {
                if (StackSignals.onInPowerStack() == SnakePowerState.Power)
                {
                    _renderer.material = rawinBowMatarial;
                }
                snakeState = SnakeState.InStack;
                StackSignals.onAddStack(this);
            }
        }

        private void OnEnterPowerMode()
        {
            if (snakeState == SnakeState.UnStack) return;
            _powerState = SnakePowerState.Power;

            _renderer.material = rawinBowMatarial;
            Invoke("ClosePowerMode", 5f);
        }

        private void ClosePowerMode()
        {
            if (_powerState != SnakePowerState.Power) return;
            _powerState = SnakePowerState.Normal;
            _renderer.material = standartMaterial;
        }

        public override void GradiantMaterial(float min, float max)
        {
            if (_powerState == SnakePowerState.Power) return;
            _renderer.material.SetColor("_BaseColor", GradiantSystemManager.GetGradiantColorByPersentage((float)min, (float)max, GradianType.Sneak));
        }
    }

    public enum SnakePowerState
    {
        Normal,
        Power
    }

    public enum SnakeState
    {
        InStack,
        UnStack
    }
}