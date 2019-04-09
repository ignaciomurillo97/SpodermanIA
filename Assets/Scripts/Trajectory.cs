using UnityEngine;
using System.Collections;

public abstract class Trajectory
{
    public bool done;
    public abstract Vector3 nextStep();
}

public class idleTrajectory : Trajectory
{
    public float steps;
    public float currStep;
    public Vector3 position;

    public idleTrajectory(Vector3 position, int steps)
    {
        this.steps = steps;
        currStep = 0;
        this.position = position;
    }

    override
    public Vector3 nextStep()
    {
        if (currStep < steps)
        {
            float t = (float)currStep / steps;
            currStep++;
            return position;
        }
        this.done = true;
        return position;
    }
}

public class BezierTrajectory : Trajectory
{
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;
    int steps;
    int currStep;

    public BezierTrajectory(Vector3 p1, Vector3 p2, Vector3 p3, int steps)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
        this.steps = steps;
        this.currStep = 0;
        this.done = false;
    }

    override
    public Vector3 nextStep()
    {
        if (currStep < steps)
        {
            float t = (float)currStep / steps;
            currStep++;
            return Bezier(p1, p2, p3, t);
        }
        this.done = true;
        return p3;
    }

    private Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        return Mathf.Pow((1 - t), 2) * p1 + 2 * (1 - t) * t * p2 + Mathf.Pow(t, 2) * p3;
    }
}
