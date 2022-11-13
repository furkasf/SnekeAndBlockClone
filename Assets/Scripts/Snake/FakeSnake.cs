using GenericPoolSystem;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Snake
{
    public class FakeSnake : MonoBehaviour
    {
        [SerializeField] private TMP_Text snakeNumberText;
        [SerializeField] private int snakeNumber;

        private void Awake()
        {
            snakeNumberText.text = snakeNumber.ToString();
        }

        private void GetSnakesFromPool()
        {
            for (int i = 0; i < snakeNumber; i++)
            {
                PoolSignals.onGetObjectFormPool("Snake");
            }
            gameObject.SetActive(false); ;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("First"))
            {
                Debug.Log("fake Snake touch the first");
                GetSnakesFromPool();
            }
        }

        private void OnDisable()
        {
            snakeNumber = Random.Range(0, 10);
            snakeNumberText.text = snakeNumber.ToString();
        }
    }
}