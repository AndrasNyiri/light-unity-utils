using System;

namespace LightUtils.Event
{
  public class CalledEvent
  {
    private bool _wasCalled;
    private event Action _callback;

    public void Invoke()
    {
      _callback?.Invoke();
      _wasCalled = true;
    }

    public void Subscribe(Action callback)
    {
      _callback += callback;
      if (_wasCalled) callback();
    }

    public void UnSubscribe(Action callback)
    {
      _callback -= callback;
    }

    public static CalledEvent operator +(CalledEvent calledEvent, Action callback)
    {
      calledEvent.Subscribe(callback);
      return calledEvent;
    }

    public static CalledEvent operator -(CalledEvent calledEvent, Action callback)
    {
      calledEvent.UnSubscribe(callback);
      return calledEvent;
    }
  }
}