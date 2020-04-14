using UnityEngine;

public class SteerToTargetCommand : Command
{
    private GameObject target;
    private float rotationSpeed;
    
    public SteerToTargetCommand(GameObject target, float rotationSpeed)
    {
        this.target = target;
        this.rotationSpeed = rotationSpeed;
    }

    public override void Execute(GameObject gameObject, float scale = 1f)
    {
        Debug.Log(target.transform.position);
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.transform.position - gameObject.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotationSpeed * scale * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        gameObject.transform.rotation = Quaternion.LookRotation(newDirection);
    }
}