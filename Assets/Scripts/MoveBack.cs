using UnityEngine;

public class MoveBack : MonoBehaviour
{
       public bool isObsitical;
       void Update()
       {
              if (transform.position.x >= (isObsitical?3.15f:7))
              {
                     transform.position = new Vector3(isObsitical?-60.02501f:-56.17501f, transform.position.y, transform.position.z);
                     if (isObsitical)
                     {
                            GameController.controller.scoreLong++;
                            PlayerPrefs.SetString("score",GameController.controller.scoreLong.ToString());
                            transform.transform.localRotation = Quaternion.Euler(
                                   0,
                                   Random.Range(0f,360f),
                                   0);
                     }
              }

       }
    
}