using GenericPoolSystem.PoolData;
using UnityEngine;

namespace GenericPoolSystem.PoolData
{
    [CreateAssetMenu(menuName = "PoolData/CD_SnakePool", fileName = "CD_SnakePool")]
    public class CD_SnakePool : CD_AbstractPool<string>
    {
        private CD_SnakePool()
        {
            Key = "Snake";
            InitialSize = 30;
            IsExtensible = true;
        }
    }

    
}