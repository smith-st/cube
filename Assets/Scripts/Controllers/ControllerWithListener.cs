using UnityEngine;

public abstract class ControllerWithListener<T>:MonoBehaviour
{
    protected T _listener;

    public void AddListener(T listener)
    {
        _listener = listener;
    }
}