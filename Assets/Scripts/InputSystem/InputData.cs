using UnityEngine;

namespace MyInput
{
    [CreateAssetMenu(fileName = "InputData", menuName = "InputData", order = 0)]
    public class InputData : ScriptableObject
    {
        public float InputSpeed = 2f;
    }
}