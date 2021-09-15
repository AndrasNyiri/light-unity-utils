using System;
using UnityEngine;

namespace Assets.LightUtils.Pooling
{
  [Serializable]
  public class PrefabInfo
  {
    public GameObject prefab;

    [Range(1, 100)] public int count = 1;
  }
}