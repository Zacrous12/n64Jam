using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class platform : MonoBehaviour
{
    [Tooltip("The direction that the other object should be coming from for entry.")]
    [SerializeField] 
    private Vector3 entryDirection = Vector3.up;
    [Tooltip("Should the entry direction be used as a local direction?")]
    [SerializeField] 
    private bool localDirection = false;
    [Tooltip("How large should the trigger be in comparison to the original collider?")]
    [SerializeField] 
    private Vector3 triggerScale = Vector3.one * 1.25f;
    [Tooltip("The collision will activate only when the penetration depth of the intruder is smaller than this threshold.")]
    [SerializeField]
    private float penetrationDepthThreshold = 0.2f;

    [Tooltip("The original collider. Will always be present thanks to the RequireComponent attribute.")]
    private new BoxCollider collider = null;
    [Tooltip("The trigger that we add ourselves once the game starts up.")]
    private BoxCollider collisionCheckTrigger = null;

    public Vector3 PassthroughDirection => localDirection ? transform.TransformDirection(entryDirection.normalized) : entryDirection.normalized;

    private playerController playerController;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        playerController = GameObject.Find("Player").GetComponent<playerController>();
        whatIsGround = LayerMask.GetMask("Ground");

        // Adding the BoxCollider and making sure that its sizes match the ones
        // of the OG collider.
        collisionCheckTrigger = gameObject.AddComponent<BoxCollider>();
        collisionCheckTrigger.size = new Vector3(
            collider.size.x * triggerScale.x,
            collider.size.y * triggerScale.y,
            collider.size.z * triggerScale.z
        );
        collisionCheckTrigger.center = collider.center;
        collisionCheckTrigger.isTrigger = true;
    }

    private void OnValidate()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    private void OnTriggerStay(Collider other)
    {
        TryIgnoreCollision(other);
    }

    public void TryIgnoreCollision(Collider other)
    {
        // Simulate a collision between our trigger and the intruder to check
        // the direction that the latter is coming from. The method returns true
        // if any collision has been detected.
        if (playerController.isHoldDown){
            Physics.IgnoreCollision(collider, other, true);
            return;
        } 

        if (Physics.ComputePenetration(
            collisionCheckTrigger, collisionCheckTrigger.bounds.center, transform.rotation,
            other, other.bounds.center, other.transform.rotation,
            out Vector3 collisionDirection, out float penetrationDepth))
        {
            float dot = Vector3.Dot(PassthroughDirection, collisionDirection);

            // Opposite direction; passing not allowed.
            if (dot < 0)
            {
                // Activate collison only once the intruder is close enough to the trigger border, to avoid teleportation
                if(penetrationDepth < penetrationDepthThreshold) 
                {
                    // Making sure that the two object are NOT ignoring each other.
                    Physics.IgnoreCollision(collider, other, false);
                }
            }
            else
            {
                // Making the colliders ignore each other, and thus allow passage
                // from one way.
                Physics.IgnoreCollision(collider, other, true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Instead of directly using the transform.position, we're using the
        // collider center, converted into global position. The way I did it
        // in the video made it easier to write, but the on-screen drawing would
        // not take in consideration the actual offset of the collider.
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.TransformPoint(collider.center), PassthroughDirection * 2);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.TransformPoint(collider.center), -PassthroughDirection * 2);
    }
}