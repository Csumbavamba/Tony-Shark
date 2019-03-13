using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void Awake()
    {
        target = FindObjectOfType<AvatarControl>().transform;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.y = desiredPosition.y;
        transform.position = smoothedPosition;
        transform.LookAt(target);
    }
}
