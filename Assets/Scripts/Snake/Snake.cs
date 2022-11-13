using DG.Tweening;
using GenericPoolSystem;
using StackManager;
using UnityEngine;

namespace Snake
{
    public class Snake : AbstractSnake
    {
        public override Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }

        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private Material standartMaterial;
        [SerializeField] private Material rawinBowMatarial;
        [SerializeField] private SnakeState snakeState = SnakeState.UnStack;

        private Vector3 _originalScale;
        private SnakePowerState _powerState = SnakePowerState.Normal;
        private bool _firstTimeIgnored;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            if (_firstTimeIgnored)
            {
                Invoke("AddSelfToStack", 0.1f);
            }
            _firstTimeIgnored = true;
        }

        private void OnDisable()
        {
            SnakeSelfReset();
        }

        private void SnakeSelfReset()
        {
            //spawn partical from pool and put sneake to pool
            //reset scale
            if (snakeState == SnakeState.InStack)
            {
                tag = "Untagged";
                transform.DOScale(0.1f, 0.1f).OnComplete(() =>
                {
                    //spawn paricule
                    GameObject partical = PoolSignals.onGetObjectFormPool("SnakePartical");
                    partical.transform.position = transform.position;
                    StackSignals.onRemoveStack();
                    transform.localScale = _originalScale;
                    snakeState = SnakeState.UnStack;
                });
            }
        }

        private void AddSelfToStack()
        {
            Debug.Log("addself called");
            if (snakeState == SnakeState.UnStack)
            {
                snakeState = SnakeState.InStack;
                StackSignals.onAddStack(this);
            }
        }

        //drive abstract function from it
        private void OnEnterPowerMode()
        {
            if (snakeState == SnakeState.UnStack) return;
            _powerState = SnakePowerState.Power;
            //apply raninbow
            Invoke("ClosePowerMode", 5f);
        }

        private void ClosePowerMode()
        {
            if (_powerState != SnakePowerState.Power) return;
            _powerState = SnakePowerState.Normal;
            //return normal material
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