using System;
using UnityEngine;

namespace LightUtils.Invoking
{
  public class FloatTween : Tween<float>
  {
    public FloatTween(float startValue, Action<float> setter, float endValue, float duration) : base(startValue, setter, endValue, duration)
    {
    }

    public override bool Tick(float dt)
    {
      delay += dt;
      if (delay < 0f)
      {
        return false;
      }

      float f;
      if (Duration <= 0f)
      {
        Factor = 1f;
        f = Factor;
      }
      else
      {
        Factor += dt * (1f / Duration);
        f = Factor;
        if (ease != null)
        {
          f = ease(f);
        }
        f = Mathf.Clamp01(f);
      }
      
      Value = Mathf.Lerp(startValue, endValue, f);
      setter(Value);
      return IsCompleted();
    }

    
  }
}