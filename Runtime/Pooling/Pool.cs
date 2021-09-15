using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LightUtils.Pooling
{
  public class Pool : MonoBehaviour
  {
    private readonly Queue<PooledObject> _container = new Queue<PooledObject>();
    public string Id => Prefab.name;
    public GameObject Prefab { get; private set; }
    public int ObjectCount => _container.Count;

    public void Init(GameObject prefab)
    {
      Prefab = prefab;
    }

    public void Create()
    {
      GameObject newObject = CreateNewObject(Vector3.zero, Quaternion.identity, transform);
      Add(newObject);
    }

    public void Add(GameObject go)
    {
      go.SetActive(false);
      var pooledObjectComponent = InitGo(go);
      Transform goTrans = go.transform;
      if (LightPool.UseReparenting) goTrans.SetParent(transform);

      goTrans.position = new Vector3(0f, -1000f, 0f);
      goTrans.rotation = Quaternion.identity;
      _container.Enqueue(pooledObjectComponent);
    }

    private PooledObject InitGo(GameObject go)
    {
      PooledObject pooledObjectComponent = go.GetComponent<PooledObject>();

      if (pooledObjectComponent == null) pooledObjectComponent = go.AddComponent<PooledObject>();

      pooledObjectComponent.Init(this);

      return pooledObjectComponent;
    }

    public GameObject Get(Vector3 position, Quaternion rotation, Transform parent)
    {
      GameObject go;
      if (_container.Any())
      {
        var pooledObject = _container.Dequeue();
        Transform poolTrans = pooledObject.transform;
        poolTrans.SetParent(parent);
        poolTrans.position = position;
        poolTrans.rotation = rotation;
        go = pooledObject.gameObject;
        go.SetActive(true);
        pooledObject.Reuse();
      }
      else
      {
        go = CreateNewObject(position, rotation, parent);
      }

      return go;
    }

    private GameObject CreateNewObject(Vector3 position, Quaternion rotation, Transform parent)
    {
      var go = Instantiate(Prefab, position, rotation, parent);
      InitGo(go);
      return go;
    }
  }
}