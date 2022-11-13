using Snake;
using StackManager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using Utalitys;

namespace StackManager
{
    public class StackLerpMoveCommand
    {
        private List<AbstractSnake> _collectables;
        private LerpData _lerpData;
        private const float _max = 5;
        private const float _min = -4.9f;

        public StackLerpMoveCommand(ref List<AbstractSnake> collectables, ref LerpData lerpData)
        {
            _collectables = collectables;
            _lerpData = lerpData;
        }

        public void Execute()
        {
            if (_collectables.Count > 0)
            {
                _collectables[0].Rigidbody.velocity = new Vector3(_lerpData.LerpMoveDirection.x * _lerpData.LerpSpeed, _collectables[0].Rigidbody.velocity.y,
               _lerpData.LerpForwardSpeed);

                _collectables[0].Rigidbody.CalculateNextFramePosition();
            }

            if (_collectables.Count > 1)
            {
                for (int i = 1; i < _collectables.Count; i++)
                {

                    Vector3 nextTarget = new Vector3(_collectables[i - 1].Rigidbody.transform.position.x, _collectables[i - 1].Rigidbody.transform.position.y, _collectables[i - 1].Rigidbody.transform.position.z - _lerpData.LerpDistanceOffset.z);
                    _collectables[i].Rigidbody.transform.position = Vector3.Lerp(_collectables[i].Rigidbody.position, nextTarget, _lerpData.LerpQueueSpeed * Time.deltaTime);

                }
            }
        }

       
    }
}

