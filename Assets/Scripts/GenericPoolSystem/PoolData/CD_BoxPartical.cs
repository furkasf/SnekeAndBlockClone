using UnityEngine;

namespace GenericPoolSystem.PoolData
{
    [CreateAssetMenu(menuName = "PoolData/CD_BoxPartical", fileName = "CD_BoxPartical")]
    public class CD_BoxPartical : CD_AbstractPool<string>
    {
        private CD_BoxPartical()
        {
            Key = "BoxPartical";
            InitialSize = 30;
            IsExtensible = true;
        }
    }
}