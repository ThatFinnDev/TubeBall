using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugThreeValueInfo : MonoBehaviour
{
    [SerializeField] private Text textOne;
    [SerializeField] private Text textTwo;
    [SerializeField] private Text textThree;
    public Text title;

    public void setValues(Vector3 value)
    {
        textOne.text = value.x.ToString();
        textTwo.text = value.y.ToString();
        textThree.text = value.z.ToString();
    }
    public void setValues(object valueOne, object valueTwo, object valueThree)
    {
        textOne.text = valueOne.ToString();
        textTwo.text = valueTwo.ToString();
        textThree.text = valueThree.ToString();
    }
}
