using UnityEngine;

public abstract class Command
{
    public abstract void Execute(GameObject gameObject, float scale=1f);
}