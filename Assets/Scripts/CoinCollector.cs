using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CoinPooler.instance.AddToPool(other.transform.parent.gameObject);
            GameController.coins++;
            GameController.totalCoins++;
        }
        else if (other.CompareTag("GoldRush"))
        {
            Destroy(other.gameObject);
            GameController.instance.StartGoldRush(20f);
        }
    }
}
