using GameCore;
using Level;
using StackManager;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private GameObject IngameUi;
        [SerializeField] private GameObject StartUI;

        private int _gameScore;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            UISignals.onGameStart += OnGameStart;
            UISignals.onGameRestart += OnGameRestart;
            UISignals.onUpdateScore += OnUpdateScore;
        }

        private void UnSubscribe()
        {
            UISignals.onGameStart -= OnGameStart;
            UISignals.onGameRestart -= OnGameRestart;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void OnUpdateScore(int score = 1)
        {
            _gameScore += score;
            Debug.Log(_gameScore);
            scoreText.text = "Score :" + _gameScore;
        }

        public void OnGameStart()
        {
            StackSignals.onReadyToPlay(true);
            CoreGameSignals.setGamePlay(true);
            scoreText.text = "Score :" + _gameScore;
            IngameUi.SetActive(true);
            StartUI.SetActive(false);
        }

        private void OnGameRestart()
        {
            _gameScore = 0;
            CoreGameSignals.setGamePlay(false);
            IngameUi.SetActive(false);
            StartUI.SetActive(true);
            LevelSignals.onLoadLevel();
        }

        private void Init()
        {
            CoreGameSignals.setGamePlay(false);
            _gameScore = 0;
            IngameUi.SetActive(false);
            StartUI.SetActive(true);
        }
    }

    public enum ScoreWorlds
    {
        WoW = 2,
        CooL = 3,
        AwesomE = 4,
        MindBolwing = 5,
    }

    public static class UISignals
    {
        public static Action onGameStart;
        public static Action onGameRestart;
        public static Action onDisActivateUI;
        public static Action<int> onUpdateScore;
    }
}