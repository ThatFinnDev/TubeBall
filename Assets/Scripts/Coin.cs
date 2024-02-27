using UnityEngine;

public class Coin : MonoBehaviour
{
    static float lowestXPos = -1.662f;
    static float highestXPos = -1.4f;
    static float moveSpeed = 0.8f;
    static float rotationSpeed = 75f;
    private bool moveHigher = true;
    private Transform child;

    public void ResetPosAndRot()
    {
        moveHigher = true;
        child.localRotation = Quaternion.Euler(0,90,0);
        child.transform.localPosition = new Vector3(lowestXPos,child.transform.localPosition.y,child.transform.localPosition.z);
    }
    private void Awake() { child = transform.GetChild(0); }

    void Update()
    {
        child.Rotate(0,0,Time.timeScale*Time.deltaTime*rotationSpeed);
        if (moveHigher)
        {
            child.transform.localPosition = new Vector3(child.transform.localPosition.x+(Time.timeScale*moveSpeed*Time.deltaTime),child.transform.localPosition.y,child.transform.localPosition.z);
            if(child.transform.localPosition.x>=highestXPos)
            {
                child.transform.localPosition = new Vector3(highestXPos, child.transform.localPosition.y, child.transform.localPosition.z);
                moveHigher = false;
            }
        }
        else
        {
            child.transform.localPosition = new Vector3(child.transform.localPosition.x-(Time.timeScale*moveSpeed*Time.deltaTime),child.transform.localPosition.y,child.transform.localPosition.z);
            if(child.transform.localPosition.x<=lowestXPos)
            {
                child.transform.localPosition = new Vector3(lowestXPos, child.transform.localPosition.y, child.transform.localPosition.z);
                moveHigher = true;
            }
        }
        
        if(transform.position.x>=GameController.decoTPPosX) CoinPooler.instance.AddToPool(gameObject);
    }
}
