using DG.Tweening;
using GenericPoolSystem;
using TMPro;
using UnityEngine;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private TMP_Text blockDurabilityText;
        private int blockDurability;

        private void Awake()
        {
            blockDurability = Random.Range(9, 30);
            blockDurabilityText.text = blockDurability.ToString();
        }

        private void OnDisable()
        {
            SelfReset();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody.CompareTag("First"))
            {
                Debug.Log("player touch the block");
                DecereaseDurability();
                PoolSignals.onPutObjectBackToPool(collision.gameObject, "Snake");
                transform.DOShakePosition(1, new Vector3(0.018f, 0, 0.018f), 9, 0, false, true).SetRelative().SetEase(Ease.Linear);
            }
        }

        private void SelfReset()
        {
            blockDurability = Random.Range(9, 30);
            blockDurabilityText.text = blockDurability.ToString();
        }

        private void DecereaseDurability()
        {
            blockDurability--;
            blockDurabilityText.text = blockDurability.ToString();
            
            if (blockDurability <= 0)
            {
                GameObject partical = PoolSignals.onGetObjectFormPool("BoxPartical");
                partical.transform.position = transform.position;
                gameObject.SetActive(false);
            }
        }
    }
}