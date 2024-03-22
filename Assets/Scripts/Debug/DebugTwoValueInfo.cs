using UnityEngine;
using UnityEngine.UI;

public class DebugTwoValueInfo : MonoBehaviour
{
    [SerializeField] private Text textOne;
    [SerializeField] private Text textTwo;
    public Text title;

    public void setValues(Vector2 value)
    {
        textOne.text = value.x.ToString();
        textTwo.text = value.y.ToString();
    }
    public void setValues(object valueOne, object valueTwo)
    {
        textOne.text = valueOne.ToString();
        textTwo.text = valueTwo.ToString();
    }
}