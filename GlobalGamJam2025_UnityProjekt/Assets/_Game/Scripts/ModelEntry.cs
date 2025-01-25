using System;
using UnityEngine;

public class ModelEntry<T>
{
    private T _value;
    private Func<T, T> sanityFix = f => f;

    public T Value { get => _value; set
        {
            T newValue = sanityFix(value);
            if (newValue.Equals(_value))
            {
                return;
            }
            T oldValue = _value;
            _value = newValue;
            OnChange?.Invoke(oldValue, _value);
        }
    }
    public event Action<T, T> OnChange;

    public void SetSanityFixFunc(Func<T,T> func)
    {
        sanityFix = func;
    }
    public void ForceInvoke()
    {
        OnChange?.Invoke(Value, Value);
    }
}
