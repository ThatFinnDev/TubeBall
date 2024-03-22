using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugToggle : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] Text text;
    public bool isOn
    {
        get {return toggle.isOn;}
        set{toggle.isOn = value;}
    }
    private UnityEvent _onChange = new UnityEvent();

    public void setFunctionality(string title, bool state, UnityAction onChange)
    {
        toggle.isOn = state;
        text.text = title;
        _onChange.RemoveAllListeners();
        _onChange.AddListener(onChange);
    }
    
    public void Toggle(bool state)
    {
        _onChange.Invoke();
    }
}
