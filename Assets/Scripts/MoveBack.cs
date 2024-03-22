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
                     transform.position = new Vector3(isObstacle?transform.position.x-57:-56.17501f, transform.position.y, isObstacle?((System.Convert.ToBoolean(Random.Range(0, 2))?-1:1)*Random.Range(GameController.difficulty.minOffset,GameController.difficulty.maxOffset)):transform.position.z);

                     if (isObstacle)
                     {
                            if(!GameController.instance.goldRush)
                            {
                                   GetComponent<BoxCollider>().enabled = true;
                                   GetComponent<MeshRenderer>().enabled = true;
                            }
                            GameController.scoreLong++;
                            PlayerPrefs.SetString("score"+GameController.instance.selectedDifficulty.ToString().Replace("0",""),GameController.scoreLong.ToString());
                            transform.transform.localRotation = Quaternion.Euler(0, Random.Range(GameController.difficulty.minRotation,GameController.difficulty.maxRotation), 0);
                     }
              }
       }
}