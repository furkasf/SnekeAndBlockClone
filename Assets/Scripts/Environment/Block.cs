using Assets.Scripts;
using DG.Tweening;
using GenericPoolSystem;
using GradientSystem;
using StackManager;
using TMPro;
using UnityEngine;

namespace Environment
{
    public class Block : MonoBehaviour
    {
        private Renderer _renderer;
        [SerializeField] private GameObject rainBow;
        [SerializeField] private TMP_Text blockDurabilityText;

        [SerializeField] private BlockLevel level;
        private const float _maxDurability = 20f;
        private float blockDurability;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            AdjustChallenge();
        }

        private void Start()
        {
            _renderer.material.SetColor("_BaseColor", GradiantSystemManager.GetGradiantColorByPersentage((float)blockDurability, (float)_maxDurability, GradianType.Block));
        }

        private void OnDisable()
        {
            AdjustChallenge();
        }

        //work on construction
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody.CompareTag("First"))
            {
                //cheack if snake in power mode direcly destroy block
                if (StackSignals.onInPowerStack() == Snake.SnakePowerState.Power)
                {
                    UISignals.onUpdateScore((int)blockDurability);
                    gameObject.SetActive(false);
                    return;
                }

                collision.transform.GetChild(0).transform.parent = null;
                UISignals.onUpdateScore(1);
                PoolSignals.onPutObjectBackToPool(collision.gameObject, "Snake");
                DecereaseDurability();
                transform.DOShakePosition(0.1f, new Vector3(0.018f, 0, 0.018f), 9, 0, false, true).SetRelative().SetEase(Ease.Linear);
            }
        }

      

        private void AdjustChallenge()
        {
            level = (BlockLevel)Random.Range(0, 3);
            rainBow.SetActive(false);
            switch (level)
            {
                case BlockLevel.easy: blockDurability = Random.Range(3, 10); break;
                case BlockLevel.normal: blockDurability = Random.Range(10, 20); break;
                case BlockLevel.star: blockDurability = 15; rainBow.SetActive(true); break;
            }

            blockDurabilityText.text = blockDurability.ToString();
        }

        private void DecereaseDurability()
        {
            blockDurability--;
            blockDurabilityText.text = blockDurability.ToString();

            if (blockDurability <= 0)
            {
                if (level == BlockLevel.star)
                {
                    StackSignals.onEnterPowerState();
                }
                GameObject partical = PoolSignals.onGetObjectFormPool("BoxPartical");
                partical.transform.position = transform.position;
                gameObject.SetActive(false);
            }
        }
    }

    public enum BlockLevel
    {
        easy,
        normal,
        star
    }
}