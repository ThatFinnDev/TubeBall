using System.Collections.Generic;
using UnityEngine;

public class CoinPooler : MonoBehaviour
{
    public List<GameObject> pooledCoins;
    public GameObject coinPrefab;
    public static CoinPooler instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddToPool(GameObject obj)
    {
        Coin coin = null;
        if (obj.GetComponents<Coin>() == null)
        {
            if (obj.transform.childCount < 0) return;
            if (obj.transform.GetChild(0).GetComponent<Coin>() == null) return;
            coin = obj.transform.GetChild(0).GetComponent<Coin>();
        }
        else coin = obj.GetComponent<Coin>();

        coin.ResetPosAndRot();
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        pooledCoins.Add(obj);
    }

    public GameObject RemoveFromPool(Transform newParent)
    {
        if (pooledCoins.Count != 0)
        {
            GameObject obj = pooledCoins[pooledCoins.Count - 1];
            obj.SetActive(true);
            obj.transform.SetParent(newParent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            pooledCoins.Remove(obj);
            return obj;
        }
        return Instantiate(coinPrefab, newParent);
    }
}
