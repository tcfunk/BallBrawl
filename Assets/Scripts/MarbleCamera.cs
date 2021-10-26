using UnityEngine;

public class MarbleCamera : MonoBehaviour
{
    [SerializeField]
    private Transform lookAtTarget;
    
    [SerializeField]
    private Rigidbody followTargetRigidbody;
    private Transform followTargetTransform;
    
    [SerializeField]
    private Vector3 followOffset;
    
    [SerializeField]
    private float smoothTime = 0.3f;
    
    
    private Vector3 velocity = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        followTargetTransform = followTargetRigidbody.transform;
        transform.position = followTargetTransform.position + followOffset;

        if (lookAtTarget == null && followTargetTransform != null)
        {
            lookAtTarget = followTargetTransform;
        }
    }

    private void Update()
    {
        var targetVelocity = new Vector3(followTargetRigidbody.velocity.x, 0, followTargetRigidbody.velocity.z);
        Vector3 targetPosition;
        
        if (targetVelocity.sqrMagnitude > 0.1f)
        {
            targetVelocity.Normalize();
                                 
            targetPosition =
                followTargetTransform.position + targetVelocity * followOffset.z
                + new Vector3(followOffset.x, followOffset.y, 0);
        }
        else
        {
            targetPosition = followTargetTransform.position + followOffset;
        }
        
        transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(lookAtTarget);
    }
}
