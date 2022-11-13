using UnityEngine;

namespace GameCore
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
        }

        private void UnSubscribe()
        {
        }

        private void OnDisable()
        {
            UnSubscribe();
        }
    }
}