using System;
using UnityEngine;

public class ModelEntry<T> : MonoBehaviour
{
    private T _value;
    public T Value { get => _value; set 
        {
            if(value.Equals(_value))
            {
                return;
            }
            T oldValue = _value;
            _value = value;
            OnChange?.Invoke(oldValue, _value);
        } 
    }
    public event Action<T, T> OnChange;
}
