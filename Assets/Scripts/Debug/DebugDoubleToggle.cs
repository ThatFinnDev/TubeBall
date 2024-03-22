using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugDoubleToggle : MonoBehaviour
{
    [SerializeField] Toggle toggle1;
    [SerializeField] Toggle toggle2;
    [SerializeField] Text text;
    public bool isOn1
    {
        get {return toggle1.isOn;}
        set{toggle1.isOn = value;}
    }
    public bool isOn2
    {
        get {return toggle2.isOn;}
        set{toggle2.isOn = value;}
    }
    private UnityEvent _onChange1 = new UnityEvent();
    private UnityEvent _onChange2 = new UnityEvent();

    public void setFunctionality(string title, bool state1, UnityAction onChange1, bool state2, UnityAction onChange2)
    {
        toggle1.isOn = state1;
        toggle2.isOn = state2;
        text.text = title;
        _onChange1.RemoveAllListeners();
        _onChange1.AddListener(onChange1);
        _onChange2.RemoveAllListeners();
        _onChange2.AddListener(onChange2);
    }
    
    public void Toggle1(bool state)
    {
        _onChange1.Invoke();
    }
    public void Toggle2(bool state)
    {
        _onChange2.Invoke();
    }
}
