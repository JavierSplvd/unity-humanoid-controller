public abstract class Cooldown
{
    public abstract void Update();
    public abstract bool IsAvailable();
    public abstract void Cool();
}