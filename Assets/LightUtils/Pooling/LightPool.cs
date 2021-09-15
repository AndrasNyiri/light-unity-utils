using System.Linq;
using UnityEngine;

namespace Assets.LightUtils.Pooling
{
  public class LightPool
  {
    private static LightPoolComponent _poolComponent;

    private static LightPoolComponent PoolComponent
    {
      get
      {
        if (_poolComponent == null)
        {
          var componentGos = Object.FindObjectsOfType<LightPoolComponent>();
          if (componentGos.Any())
          {
            for (int i = 1; i < componentGos.Length; i++) Object.DestroyImmediate(componentGos[i]);

            _poolComponent = componentGos[0];
          }
          else
          {
            _poolComponent = new GameObject("[LightPool]").AddComponent<LightPoolComponent>();
          }
        }

        return _poolComponent;
      }
    }

    public static bool UseReparenting
    {
      get => PoolComponent.UseReparenting;
      set => PoolComponent.UseReparenting = value;
    }


    public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation,
      Transform parent = null)
    {
      return PoolComponent.PoolInstantiate(prefab, position, rotation, parent);
    }

    public static void Destroy(GameObject go, float delay = 0f)
    {
      PoolComponent.PoolDestroy(go, delay);
    }

    public static void PreLoad(GameObject prefab, int count)
    {
      PoolComponent.PreLoad(prefab, count);
    }
  }
}