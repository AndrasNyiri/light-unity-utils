using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LightUtils.Invoking
{
  public static class LightInvoke
  {
    private static LightInvokeComponent _invokeComponent;

    private static LightInvokeComponent InvokeComponent
    {
      get
      {
        if (_invokeComponent == null)
        {
          var componentGos = Object.FindObjectsOfType<LightInvokeComponent>();
          for (int i = 0; i < componentGos.Length; i++) Object.DestroyImmediate(componentGos[i]);
          _invokeComponent = new GameObject("[LightInvoke]").AddComponent<LightInvokeComponent>();
        }

        return _invokeComponent;
      }
    }


    public static Invokable Invoke(Action callback, float delay)
    {
      if (delay <= 0f)
      {
        callback();
        return new Invokable(callback, delay);
      }

      return InvokeComponent.Engine.InvokeOnce(callback, delay);
    }

    public static Invokable InvokeRepeating(Action callback, float interval, int repeatCount = -1)
    {
      return InvokeComponent.Engine.Invoke(callback, interval).SetInvokeCount(repeatCount);
    }

    public static Invokable InvokeWaitOneFrame(Action callback)
    {
      return InvokeComponent.Engine.InvokeOnce(callback, 0);
    }

    public static FloatTween To(float startValue, Action<float> setter, float endValue, float duration)
    {
      return InvokeComponent.Engine.To(startValue, setter, endValue, duration);
    }
  }
}