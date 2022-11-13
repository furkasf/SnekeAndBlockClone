using UnityEngine;

namespace GenericPoolSystem.PoolData
{
    [CreateAssetMenu(menuName = "PoolData/CD_SnakeParticalPool", fileName = "CD_SnakeParticalPool")]
    public class CD_SnakeParticalPool : CD_AbstractPool<string>
    {
        private CD_SnakeParticalPool()
        {
            Key = "SnakePartical";
            InitialSize = 30;
            IsExtensible = true;
        }
    }
}