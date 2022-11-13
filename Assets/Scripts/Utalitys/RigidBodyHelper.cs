using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utalitys
{
    public static class RigidBodyHelper
    {
        private static Vector3 positionAtEndOfStep;
        private static Vector3 neededVelocity;
        private static float _min = -4.9f, _max = 5;

        public static void CalculateNextFramePosition(this Rigidbody rigidbody)
        {
            // Project where our velocity will take us by the end of the frame.
            positionAtEndOfStep = rigidbody.position + rigidbody.velocity * Time.deltaTime;

            // Limit that projected position to within our allowed bounds.
            positionAtEndOfStep.x = Mathf.Clamp(positionAtEndOfStep.x, _min, _max);

            // Compute a velocity that will take us to this clamped position instead.
            neededVelocity = (positionAtEndOfStep - rigidbody.position) / Time.deltaTime;

            rigidbody.velocity = neededVelocity;
        }
    }
}
