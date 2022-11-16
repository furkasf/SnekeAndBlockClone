using GenericPoolSystem;
using UnityEngine;

namespace Assets.Scripts
{
    public class ParticuleLifeController : MonoBehaviour
    {
        [SerializeField] ParticalType particuleType;
        private bool _firtTimeActive;
        


        private void OnDisable()
        {
            if (!_firtTimeActive)
            {
                _firtTimeActive = true;
                return;
            }
            PoolSignals.onPutObjectBackToPool(gameObject, particuleType.ToString());
        }
    }
    public enum ParticalType
    {
        BoxPartical,
        SnakePartical
    }

}