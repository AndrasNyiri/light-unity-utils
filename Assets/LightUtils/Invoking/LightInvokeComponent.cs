using UnityEngine;

namespace Assets.LightUtils.Invoking
{
  public class LightInvokeComponent : MonoBehaviour
  {
    [SerializeField] private int _invokableCount;

    public InvokeEngine Engine { get; } = new InvokeEngine();

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    private void Update()
    {
      Engine.Update(Time.deltaTime);
      _invokableCount = Engine.GetInvokableCount();
    }
  }
}