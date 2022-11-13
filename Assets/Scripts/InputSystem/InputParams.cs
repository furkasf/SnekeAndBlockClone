using UnityEngine;

namespace MyInput
{
    public readonly struct InputParams
    {
        public InputParams(Vector3 _movment)
        {
            movementVector = _movment;
        }

        public readonly Vector3 movementVector;
    }
}