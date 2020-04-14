using UnityEngine;

public abstract class Invoker
{
    public abstract void Update();
    public abstract void ClearAllCommands();
    public abstract void AddCommand(Command c);
}