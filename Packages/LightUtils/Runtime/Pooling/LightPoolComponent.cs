using System.Collections.Generic;
using System.Linq;
using LightUtils.Invoking;
using UnityEngine;

namespace LightUtils.Pooling
{
  public class LightPoolComponent : MonoBehaviour
  {
    [SerializeField] private bool _useReparenting = true;

    private readonly List<(GameObject go, Invokable inv)> _destoryingGameObjects = new List<(GameObject, Invokable)>();

    private readonly Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public bool UseReparenting
    {
      get => _useReparenting;
      set => _useReparenting = value;
    }

    public GameObject PoolInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
      string id = GetId(prefab);
      if (!_pools.TryGetValue(id, out Pool pool)) pool = CreatePool(prefab, id);
      GameObject pooledObject = pool.Get(position, rotation, parent);
      return pooledObject;
    }

    private string GetId(GameObject prefab)
    {
      return prefab.name;
    }

    private Pool CreatePool(GameObject prefab, string id)
    {
      var newPool = new GameObject($"{id} Pool").AddComponent<Pool>();
      newPool.transform.parent = transform;
      newPool.Init(prefab);
      _pools.Add(id, newPool);
      return newPool;
    }


    public void PoolDestroy(GameObject go, float delay)
    {
      (GameObject go, Invokable inv) tuple = _destoryingGameObjects.FirstOrDefault(d => d.go == go);
      if (tuple != default) tuple.inv.Interrupt();

      Invokable inv = LightInvoke.Invoke(() =>
      {
        _destoryingGameObjects.RemoveAll(g => g.go == go);

        Pool pool;

        PooledObject poolObjectComponent = go.GetComponent<PooledObject>();
        if (poolObjectComponent == null)
        {
          string id = go.name.Split(' ')[0];
          if (_pools.ContainsKey(id))
          {
            pool = _pools[id];
          }
          else
          {
            Debug.LogError($"{go.name} is not a pooled object!");
            Destroy(go);
            return;
          }
        }
        else
        {
          pool = poolObjectComponent.Pool;
        }

        pool.Add(go);
      }, delay);

      if (delay > 0f) _destoryingGameObjects.Add((go, inv));
    }

    public void PreLoad(GameObject prefab, int count)
    {
      string id = GetId(prefab);
      if (!_pools.TryGetValue(id, out var pool)) pool = CreatePool(prefab, id);
      for (int i = 0; i < count; i++) pool.Create();
    }
  }
}