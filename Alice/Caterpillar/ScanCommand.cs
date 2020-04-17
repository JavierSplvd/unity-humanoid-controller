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
        for (int i = 0; i < 10; i++)
        {
            direction = new Vector3(Mathf.Sin(2 * 0.31415f * i), 0, Mathf.Cos(2 * 0.31415f * i));
            Debug.DrawRay(gameObject.transform.position + Vector3.up, direction * raycastDistance, Color.green, 0.1f);

            if (Physics.Raycast(gameObject.transform.position + Vector3.up, direction, out hit, raycastDistance, rayMask))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    Debug.Log("hit");
                    hitListener.Notify();
                    break;
                }

            }

        }

    }
}
