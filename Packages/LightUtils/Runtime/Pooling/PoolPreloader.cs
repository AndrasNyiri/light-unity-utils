using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LightUtils.Pooling
{
  public class PoolPreloader : MonoBehaviour
  {
    [SerializeField] private bool _debug = true;
    [SerializeField] private List<PrefabInfo> _prefabs;
    [SerializeField] private string _resourcePath;

    [Range(0, 100)] [SerializeField] private int _resourcePreLoadCount = 1;

    private readonly StringBuilder _debugMessage = new StringBuilder();

    private void Start()
    {
      StartMessage();

      foreach (var prefabInfo in _prefabs) Preload(prefabInfo.prefab, prefabInfo.count);

      if (!string.IsNullOrWhiteSpace(_resourcePath))
      {
        var objects = Resources.LoadAll<GameObject>(_resourcePath);
        foreach (var go in objects) Preload(go, _resourcePreLoadCount);
      }

      DumpMessage();
      Destroy(gameObject);
    }

    private void Preload(GameObject prefab, int count)
    {
      LightPool.PreLoad(prefab, count);
      if (_debug) _debugMessage.AppendLine($"- {count} {prefab.name}");
    }

    private void StartMessage()
    {
      if (_debug)
      {
        _debugMessage.AppendLine($"Pooling Preload LOG [{gameObject.name}]");
        _debugMessage.AppendLine("Preloaded:");
      }
    }

    private void DumpMessage()
    {
      if (_debug) Debug.Log(_debugMessage);
    }
  }
}