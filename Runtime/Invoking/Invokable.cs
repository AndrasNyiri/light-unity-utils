using System;

namespace LightUtils.Invoking
{
  public class Invokable : ITickable
  {
    private readonly Action _action;

    private int _defaultRepeatCount = -1;
    private float _defaultTime;
    private bool _ensureInterval;
    private bool _interrupted;

    /// <summary>
    ///   Runs the given action after the given time period.
    ///   Sling count with default value means infinite.
    /// </summary>
    /// <returns></returns>
    public Invokable(Action action, float time)
    {
      _action = action;
      _defaultTime = time;
      TimeLeft = time;
    }

    public float TimeLeft { get; private set; }

    public int CurrentRepeatCount { get; private set; } = -1;

    public event Action Complete;

    /// <summary>
    /// </summary>
    /// <param name="dt">Elapsed time since the last frame</param>
    /// <returns>The Invokable should be deleted after this method is called</returns>
    public bool Tick(float dt)
    {
      if (_interrupted) return true;
      TimeLeft -= dt;
      if (TimeLeft > 0) return false;
      if (IsRepeatCountReached()) return true;

      _action();
      if (_ensureInterval && Math.Abs(TimeLeft) > _defaultTime)
        while (_defaultTime + TimeLeft < 0)
        {
          if (IsRepeatCountReached()) return true;
          _action();
          TimeLeft += _defaultTime;
          if (IsCompleted()) return true;
        }

      TimeLeft = _defaultTime + TimeLeft;
      return IsCompleted();
    }

    public TimeSpan GetTimeTillNextComplete()
    {
      return IsCompleted() ? TimeSpan.Zero : TimeSpan.FromSeconds(TimeLeft);
    }


    public bool IsCompleted()
    {
      var completed = CurrentRepeatCount == 0;
      if (completed) Complete?.Invoke();
      return completed;
    }

    private bool IsRepeatCountReached()
    {
      return _defaultRepeatCount > -1 && CurrentRepeatCount-- <= 0;
    }

    /// <summary>
    ///   Resets everything to default values in the Invokable
    /// </summary>
    public void Reset(float newTime)
    {
      _defaultTime = newTime;
      TimeLeft = _defaultTime;
      CurrentRepeatCount = _defaultRepeatCount;
    }

    /// <summary>
    ///   Interrupts the Invokable
    /// </summary>
    public void Interrupt()
    {
      _interrupted = true;
    }

    public Invokable Invoke()
    {
      _action();
      return this;
    }

    public Invokable SetInvokeCount(int repeatCount)
    {
      _defaultRepeatCount = repeatCount;
      CurrentRepeatCount = repeatCount;
      return this;
    }

    public Invokable OnComplete(Action callback)
    {
      Complete = callback;
      return this;
    }

    public Invokable SetEnsureInterval(bool ensureInterval)
    {
      _ensureInterval = ensureInterval;
      return this;
    }
  }
}