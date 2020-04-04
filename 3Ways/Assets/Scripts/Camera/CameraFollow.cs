using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform lookTarget;


    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothPos;

        transform.LookAt(lookTarget);
    }
}
