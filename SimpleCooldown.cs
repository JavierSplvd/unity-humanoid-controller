using UnityEngine;

public class SimpleCooldown : Cooldown
{
    private float currentTime;
    private float maxTime;

    public SimpleCooldown(float maxTime)
    {
        this.maxTime = maxTime;
        currentTime = 0f;
    }

    public override void Heat()
    {
        currentTime = maxTime;
    }

    public override bool IsAvailable()
    {
        return currentTime == 0f;
    }

    public override void Update()
    {
        currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, maxTime);
    }
}