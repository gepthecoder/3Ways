using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static bool Win;

    public Transform target;
    public Transform lookTarget;
    
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (!Win)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothPos;

            transform.LookAt(lookTarget);
        }
        else
        {
            smoothSpeed = 0.0125f;
            offset = new Vector3(5, 1, 3);

            Vector3 desiredPos = target.position + offset;
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothPos;

            transform.LookAt(lookTarget);
        }
    }
}
