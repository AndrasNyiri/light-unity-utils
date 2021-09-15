using System;

namespace LightUtils.Invoking
{
  public abstract class Tween<T> : ITickable
  {
    public T endValue { get; }
    protected Action<T> setter { get; }
    protected Func<float, float> ease { get; set; }
    protected float delay { get; set; }
    protected T startValue { get; }
    public float Duration { get; set; }

    public T Value { get; protected set; }
    public float Factor { get; protected set; }

    protected Action completed;

    public Tween(T startValue, Action<T> setter, T endValue, float duration)
    {
      this.startValue = startValue;
      this.setter = setter;
      this.endValue = endValue;
      this.Duration = duration;
    }

    public abstract bool Tick(float dt);

    public Tween<T> SetEase(Func<float, float> func)
    {
      this.ease = func;
      return this;
    }

    public Tween<T> SetDelay(float value)
    {
      this.delay = -value;
      return this;
    }

    public Tween<T> OnComplete(Action callback)
    {
      completed = callback;
      return this;
    }

    public bool IsCompleted()
    {
      var isCompleted = Factor >= 1f;
      if (isCompleted)
      {
        completed?.Invoke();
      }
      return isCompleted;
    }

  }
}