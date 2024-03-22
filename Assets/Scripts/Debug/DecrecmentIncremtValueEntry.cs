using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DecrecmentIncremtValueEntry : MonoBehaviour
{
    [SerializeField] public Text text;
    public Text value;
    public UnityEvent _increment = new UnityEvent();
    public UnityEvent _decrement = new UnityEvent();

    public void setFunctionality(string title, UnityAction increment, UnityAction decrement)
    {
        text.text = title;
        _increment.RemoveAllListeners();
        _decrement.RemoveAllListeners();
        _increment.AddListener(increment);
        _decrement.AddListener(decrement);
    }
    public void setFunctionality(string title, UnityAction increment, UnityAction decrement, string defaultValue)
    {
        text.text = title;
        value.text = defaultValue;
        _increment.RemoveAllListeners();
        _decrement.RemoveAllListeners();
        _increment.AddListener(increment);
        _decrement.AddListener(decrement);
    }  
    public void Increment()
    {
        _increment.Invoke();
    }

    public void Decrement()
    {
        _decrement.Invoke();
    }
}
