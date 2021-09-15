using UnityEngine;

namespace Assets.LightUtils.Curve.Demo
{
  public class QuadraticSplineDemo : MonoBehaviour
  {
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _a;
    [SerializeField] private Transform _b;
    [SerializeField] private Transform _pivot;

    [Range(0f, 1f)] [SerializeField] private float _factor;


    private void Update()
    {
      var pointData = Spline.QuadraticSplineEvaluate(_a.position, _b.position, _pivot.position, _factor);
      _target.position = pointData.pos;
      _target.rotation = pointData.rotation;
    }
  }
}