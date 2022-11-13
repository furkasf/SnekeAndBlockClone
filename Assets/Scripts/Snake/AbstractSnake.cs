using UnityEngine;

namespace Snake
{
    public abstract class AbstractSnake : MonoBehaviour
    {
        public abstract Rigidbody Rigidbody { get; set; }

    }
}