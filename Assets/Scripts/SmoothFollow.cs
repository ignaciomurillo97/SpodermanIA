using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;
    void Update()
    {
        if (target != null)
        {
            // Define a target position above and behind the target transform
            targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
        } else
        {
            targetPosition = new Vector3(0, 2, -10);
        }

        // Smoothly move the camera towards that target position
        transform.LookAt(target.position);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
