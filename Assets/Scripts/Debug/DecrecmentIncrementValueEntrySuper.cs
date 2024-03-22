using UnityEngine.Events;

public class DecrecmentIncrementValueEntrySuper : DecrecmentIncremtValueEntry
{
    private UnityEvent _superincrement = new UnityEvent();
    private UnityEvent _superdecrement = new UnityEvent();

    public void setFunctionality(string title, UnityAction increment, UnityAction decrement, UnityAction superIncrement, UnityAction superDecrement)
    {
        text.text = title;
        _increment.RemoveAllListeners();
        _decrement.RemoveAllListeners();
        _superincrement.RemoveAllListeners();
        _superdecrement.RemoveAllListeners();
        _superincrement.AddListener(superIncrement);
        _decrement.AddListener(decrement);
        _increment.AddListener(increment);
        _superdecrement.AddListener(superDecrement);
    }
    public void setFunctionality(string title, UnityAction increment, UnityAction decrement, UnityAction superIncrement, UnityAction superDecrement, string defaultValue)
    {
        text.text = title;
        value.text = defaultValue;
        _increment.RemoveAllListeners();
        _decrement.RemoveAllListeners();
        _superincrement.RemoveAllListeners();
        _superdecrement.RemoveAllListeners();
        _superincrement.AddListener(superIncrement);
        _decrement.AddListener(decrement);
        _increment.AddListener(increment);
        _superdecrement.AddListener(superDecrement);
    }  
    public void Increment()
    {
        _increment.Invoke();
    }

    public void Decrement()
    {
        _decrement.Invoke();
    }
    public void SuperIncrement()
    {
        _superincrement.Invoke();
    }

    public void SuperDecrement()
    {
        _superdecrement.Invoke();
    }
}
