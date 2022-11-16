using UnityEngine;

namespace GameCore
{
    public class GameManager : MonoBehaviour
    {
        private bool isGamePlay;

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
            CoreGameSignals.onGamePlay += OnGamePlay;
            CoreGameSignals.setGamePlay += SetGamePlaying;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.onGamePlay -= OnGamePlay;
            CoreGameSignals.setGamePlay -= SetGamePlaying;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private bool OnGamePlay() => isGamePlay;

        private void SetGamePlaying(bool isPlay)
        {
            isGamePlay = isPlay;
        }
    }
}