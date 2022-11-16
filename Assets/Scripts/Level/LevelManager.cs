using GenericPoolSystem;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> levels = new List<Transform>();
        [SerializeField] private Transform levelHolder;

        private LevelLoaderCommand _levelLoaderCommand;
        private ClearActiveLevelCommand _clearActiveLevelCommand;

        private const float _levelLength = 102.2f;

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
            LevelSignals.onGetNextLevel += OnGetNextLevel;
            LevelSignals.onLoadLevel += OnLoadLevel;
            LevelSignals.onClearLevel += OnClearLevel;
        }

        private void UnSubscribe()
        {
            LevelSignals.onGetNextLevel -= OnGetNextLevel;
            LevelSignals.onLoadLevel -= OnLoadLevel;
            LevelSignals.onClearLevel -= OnClearLevel;
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        [ContextMenu(nameof(OnGetNextLevel))]
        private void OnGetNextLevel()
        {
            GameObject level = PoolSignals.onGetObjectFormPool("Level");
            level.transform.position += new Vector3(0, 0, _levelLength);
            levels.Add(level.transform);
            RemoveLevel();
        }

        private void RemoveLevel()
        {
            if (levels.Count <= 1) return;

            if (levels.Count > 2)
            {
                PoolSignals.onPutObjectBackToPool(levels[0].gameObject, "Level");
                levels.RemoveAt(0);
                levels.TrimExcess();
            }
        }

        private void OnLoadLevel()
        {
            _levelLoaderCommand.LevelLoad(1, ref levelHolder);
        }

        private void OnClearLevel()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                PoolSignals.onPutObjectBackToPool(levels[i].gameObject, "Level");
            }

            levels.Clear();

            _clearActiveLevelCommand.ClearActiveLevelAsync(ref levelHolder);
            levels.TrimExcess();
        }

        private void Init()
        {
            _levelLoaderCommand = new LevelLoaderCommand();
            _clearActiveLevelCommand = new ClearActiveLevelCommand();
            _levelLoaderCommand.LevelLoad(1, ref levelHolder);
            levels.Add(levelHolder.GetChild(0).GetChild(0));
        }
    }
}