using UnityEngine;

public class AlignCommand : Command
{
    private float rotationSpeed;
    private Transform targetTransform;

    public AlignCommand(float rotationSpeed, Transform targetTransform)
    {
        this.rotationSpeed = rotationSpeed;
        this.targetTransform = targetTransform;
    }

    public override void Execute(GameObject gameObject, float scale = 1f)
    {
        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * scale * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetTransform.forward, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }
}