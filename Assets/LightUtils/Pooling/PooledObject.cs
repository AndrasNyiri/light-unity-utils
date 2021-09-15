using System;
using UnityEngine;

namespace Assets.LightUtils.Pooling
{
  public class PooledObject : MonoBehaviour
  {
    [SerializeField] private Pool _pool;

    private Rigidbody _rigidbody;

    public Pool Pool
    {
      get => _pool;
      private set => _pool = value;
    }

    public bool IsReused { get; private set; }

    private void Start()
    {
      _rigidbody = GetComponent<Rigidbody>();
    }

    public event Action Reused;

    public void Init(Pool pool)
    {
      Pool = pool;
    }

    public void Reuse()
    {
      IsReused = true;
      ResetPooledObject();
      Reused?.Invoke();
    }

    public void OnReused(Action callback)
    {
      Reused = callback;
    }

    private void ResetPooledObject()
    {
      if (_rigidbody != null)
      {
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        _rigidbody.angularVelocity = new Vector3(0f, 0f, 0f);
      }
    }
  }
}