using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    [SerializeField] Text text;
    private UnityEvent _onPress = new UnityEvent();
    public void setFunctionality(string title, UnityAction onPress)
    {
        text.text = title;
        _onPress.RemoveAllListeners();
        _onPress.AddListener(onPress);
    }
    public void Press()
    {
        _onPress.Invoke();
    }
}