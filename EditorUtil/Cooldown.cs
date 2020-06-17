public abstract class Cooldown
{
    public abstract void Update();
    public abstract bool IsAvailable();
    public abstract void Heat();
    public abstract float GetCurrentTime();
}