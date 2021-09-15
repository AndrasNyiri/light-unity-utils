using UnityEngine;

namespace Assets.LightUtils.Curve
{
  public struct SplinePointData
  {
    public Vector3 pos;
    public Vector3 forward;
    public Quaternion rotation => Quaternion.LookRotation(forward);
    public Vector3 left => new Vector2(-forward.y, forward.x);

    public float currentPathLength;
    public float time;

    public SplinePointData(Vector3 pos, Vector3 forward)
    {
      this.pos = pos;
      this.forward = forward;
      currentPathLength = 0f;
      this.currentPathLength = 0;
      this.time = 0;
    }
  }

  public static class Spline
  {
    public static SplinePointData QuadraticSplineEvaluate(Vector3 a, Vector3 b, Vector3 pivot, float t)
    {
      var p0 = Vector3.Lerp(a, pivot, t);
      var p1 = Vector3.Lerp(pivot, b, t);
      var dir = (p1 - p0).normalized;
      var pos = Vector3.Lerp(p0, p1, t);
      return new SplinePointData(pos, dir);
    }

    public static SplinePointData LinearSplineEvaluate(Vector3 a, Vector3 b, float t)
    {
      return new SplinePointData(Vector3.Lerp(a, b, t), (b - a).normalized);
    }
  }
}