using UnityEngine;

namespace Assets.LightUtils.Pooling.Helpers
{
  public class PooledAutoPSDestroy : MonoBehaviour
  {
    private void OnEnable()
    {
      ParticleSystem system = GetComponent<ParticleSystem>();
      var main = system.main;
      var timeLeft = main.startLifetimeMultiplier + main.duration;
      LightPool.Destroy(gameObject, timeLeft);
    }
  }
}