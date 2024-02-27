using UnityEngine;
using Random = UnityEngine.Random;

public class MoveBack : MonoBehaviour
{
       public bool isObstacle;
       Vector3 defaultPos;
       private Quaternion defaultRot;
       private void Awake()
       { 
              defaultPos = transform.position;
              defaultRot = transform.rotation;
       }

       public void ResetInfo()
       {
              transform.position = defaultPos;
              transform.rotation = defaultRot;
       }
       void Update()
       {
              if (transform.position.x >= (isObstacle?GameController.obstacleTPPosX:GameController.decoTPPosX))
              {
                     System.Convert.ToBoolean(Random.Range(0, 2));

                     transform.position = new Vector3(isObstacle?-60.02501f:-56.17501f, transform.position.y, isObstacle?((System.Convert.ToBoolean(Random.Range(0, 2))?-1:1)*Random.Range(GameController.difficulty.minOffset,GameController.difficulty.maxOffset)):transform.position.z);
                     if (isObstacle)
                     {
                            GameController.scoreLong++;
                            PlayerPrefs.SetString("score"+GameController.instance.selectedDifficulty.ToString().Replace("0",""),GameController.scoreLong.ToString());
                            transform.transform.localRotation = Quaternion.Euler(0, Random.Range(GameController.difficulty.minRotation,GameController.difficulty.maxRotation), 0);
                     }
              }
       }
}