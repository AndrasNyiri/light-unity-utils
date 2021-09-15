using System;
using System.Collections.Generic;

namespace Assets.LightUtils.Invoking
{
  public class InvokeEngine
  {
    private readonly List<ITickable> _tickables = new List<ITickable>();

    public Invokable Invoke(Action action, float interval)
    {
      return AddInvokable(new Invokable(action, interval));
    }

    public Invokable InvokeOnce(Action action, float interval)
    {
      return Invoke(action, interval).SetInvokeCount(1);
    }

    public FloatTween To(float startValue, Action<float> setter, float endValue, float duration)
    {
      var tween = new FloatTween(startValue, setter, endValue, duration);
      _tickables.Add(tween);
      return tween;
    }

    private Invokable AddInvokable(Invokable invokable)
    {
      _tickables.Add(invokable);
      return invokable;
    }

    public void Update(float deltaTime)
    {
      if (_tickables.Count == 0) return;
      for (var i = _tickables.Count - 1; i >= 0; i--)
      {
        var invokable = _tickables[i];
        if (invokable.Tick(deltaTime)) _tickables.Remove(invokable);
      }
    }

    public int GetInvokableCount()
    {
      return _tickables.Count;
    }
  }
}