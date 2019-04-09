using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpidermanController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accelerationDistanceSquared;
    private Animator animator;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float smoothTime;
    private Vector3 velocity = Vector3.zero;

    [SerializeField]
    private float epsilonDistSquared;
    [SerializeField]
    private Vector3 targetPosition;
    private List<Vector3> path;

    private Vector3 prevPosition;
    private TrajectoryManager tm;

    [SerializeField]
    private float runDistance;
    [SerializeField]
    private float jumpDistance;
    [SerializeField]
    private float jumpHeight;

    void Start()
    {
        animator = GetComponent<Animator>();
        prevPosition = transform.position;
        tm = new TrajectoryManager();
    }

    void Update()
    {
        prevPosition = transform.position;

        if (!tm.done)
        {
            transform.position = tm.getNextStep();
        }

        Vector3 landVelocity = getLandVelocity();
        animator.SetFloat("landSpeed", landVelocity.magnitude);

        setOrientationWithVelocity(landVelocity);
    }

    void setOrientationWithVelocity(Vector3 landVelocity)
    {
        if (landVelocity.magnitude > 0.1)
        {
            float direction = Mathf.Atan2(landVelocity.x, landVelocity.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * direction;
        }
    }

    Vector3 getLandVelocity()
    {
        Vector3 velocity = transform.position - prevPosition;
        velocity.y = 0;
        return velocity;
    }

    private void followPath(List<Vector3> path)
    {
        this.path = path;
        this.targetPosition = path[0];
    }

    public void FootL() // Rig animation compatibility, intentionally left empty
    {
        return;
    }

    public void FootR() // Rig animation compatibility, intentionally left empty
    {
        return;
    }

    public void Land() // Rig animation compatibility, intentionally left empty
    {
        return;
    }
}
