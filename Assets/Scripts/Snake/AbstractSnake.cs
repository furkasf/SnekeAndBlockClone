using UnityEngine;

namespace Snake
{
    public abstract class AbstractSnake : MonoBehaviour
    {
        public abstract Rigidbody Rigidbody { get; set; }
        public abstract SnakePowerState PowerState { get; set; }
        public abstract void GradiantMaterial(float min, float max);

    }
}