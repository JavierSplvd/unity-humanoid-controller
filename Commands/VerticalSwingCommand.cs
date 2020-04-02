using UnityEngine;

public class VerticalSwingCommand : Command
{
    private float amplitude;
    private float angularVel;

    private float currentDelta;
    private float previousDelta;
    private Vector3 currentDeltaVector;
    public VerticalSwingCommand(float amplitude, float angularVel)
    {
        this.amplitude = amplitude;
        this.angularVel = angularVel;

        currentDelta = 0f;
        currentDeltaVector = new Vector3(0,0,0);
        Debug.Log(currentDeltaVector);

    }

    public override void Execute(GameObject gameObject)
    {
        currentDelta = Mathf.Sin(angularVel * Time.time) * amplitude;
        currentDeltaVector.y = currentDelta - previousDelta;
        gameObject.transform.Translate(currentDeltaVector, Space.Self);
        previousDelta = currentDelta;
    }
}