using UnityEngine;

namespace StackManager
{
    [CreateAssetMenu(fileName = "LerpData", menuName = "LerpData")]
    public class LerpData : ScriptableObject
    {
        public float LerpSpeed;
        public float LerpForwardSpeed;
        public float LerpQueueSpeed;
        public float LerpSpeedMultiplier;
        public float LerpTime;
        public Vector3 LerpMoveDirection;
        public Vector3 LerpDistanceOffset = Vector3.zero;
    }
}