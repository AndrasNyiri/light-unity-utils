﻿using UnityEngine;

namespace Assets.LightUtils.Pooling.Helpers
{
  public class PooledAutoDestroy : MonoBehaviour
  {
    [SerializeField] private float _delay;


    private void OnEnable()
    {
      LightPool.Destroy(gameObject, _delay);
    }
  }
}