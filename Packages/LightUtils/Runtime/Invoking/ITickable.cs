namespace LightUtils.Invoking
{
  public interface ITickable
  {
    /// <summary>
    /// </summary>
    /// <param name="dt">Elapsed time since the last frame</param>
    /// <returns>The ITickable should be deleted after this method is called</returns>
    bool Tick(float dt);
  }
}