using UnityEngine;

public class ScanCommand : Command
{
    private LayerMask rayMask;
    private float raycastDistance;
    private RaycastHit hit;
    private Vector3 direction = Vector3.zero;
    private Listener hitListener;

    public ScanCommand(LayerMask rayMask, float raycastDistance, Listener listener)
    {
        this.rayMask = rayMask;
        this.raycastDistance = raycastDistance;
        hitListener = listener;
    }

    public override void Execute(GameObject gameObject, float scale = 1)
    {
        for(int i = 0; i<360; i++)
        {
            direction = new Vector3(Mathf.Sin(i), 0, Mathf.Cos(i));
            if (Physics.Raycast(gameObject.transform.position, direction, out hit, raycastDistance, rayMask))
            {
                Debug.DrawRay(gameObject.transform.position, direction * raycastDistance, Color.green, 0.1f);
                Debug.Log("hit");
                hitListener.Notify();
                break;
            }

        }
        
    }
}
