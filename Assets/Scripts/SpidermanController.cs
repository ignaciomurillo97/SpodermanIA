using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpidermanController : MonoBehaviour
{
    private Animator animator;
    public GridNodes Grid;

    //private List<Node> path;

    private Vector3 prevPosition;
    private TrajectoryManager tm;

    [SerializeField]
    private float runDistance;
    [SerializeField]
    private float jumpHeight;

    void Awake()
    {
        animator = GetComponent<Animator>();
        prevPosition = transform.position;
        tm = new TrajectoryManager();
    }

    void Start()
    {
    }

    void Update()
    {
        prevPosition = transform.position;
        jumpHeight = Grid.NodeSize/4;
        runDistance = Grid.NodeSize/8;

        if (!tm.done)
        {
            transform.position = tm.getNextStep();
        }

        setAnimationWithState();
        Vector3 landVelocity = getLandVelocity();

        setOrientationWithVelocity(landVelocity);
    }

    void setAnimationWithState()
    {
        switch(tm.currState())
        {
            case "run":
                animator.SetInteger("state", 1);
                break;
            case "jump":
                animator.SetInteger("state", 2);
                break;
            case "fall":
                animator.SetInteger("state", 3);
                break;
            case "land":
                animator.SetInteger("state", 4);
                break;
            case "done":
                animator.SetInteger("state", 5);
                break;
        }
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

    public void followPath(List<Node> path)
    {
        if (path.Count < 2) return;
        print(path.Count);
        tm.setTrajectories(generateTrajectory(path));
    }

    private List<Trajectory> generateTrajectory(List<Node> Path)
    {
        List<Trajectory> t = new List<Trajectory>();

        startTrajectory(Path, t);
        for (int i = 1; i < Path.Count - 1; i ++)
        {
            float sizeRation = Grid.NodeSize;
            Vector3 a = getRoofEdge(Path[i].vPosition, Path[i - 1].vPosition) + Vector3.up * sizeRation;
            Vector3 b = Path[i].vPosition+ Vector3.up * sizeRation;
            Vector3 c = getRoofEdge(Path[i].vPosition, Path[i + 1].vPosition) + Vector3.up * sizeRation;
            Vector3 e = getRoofEdge(Path[i + 1].vPosition, Path[i].vPosition)+ Vector3.up * sizeRation;
            Vector3 d = jumpApex(c, e) + Vector3.up * sizeRation;

            t.Add(new BezierTrajectory(a, b, c, 50, "run"));
            t.Add(new idleTrajectory(c, 8, "jump"));
            t.Add(new BezierTrajectory(c, d, e, 50, "fall"));
            t.Add(new idleTrajectory(e, 8, "land"));
        }
        return t;
    }

    private void startTrajectory(List<Node> path, List<Trajectory> t)
    {
        float sizeRation = Grid.NodeSize;
        Vector3 a = path[0].vPosition + Vector3.up * sizeRation;
        Vector3 b = getRoofEdge(path[0].vPosition, path[1].vPosition) + Vector3.up * sizeRation;
        Vector3 c = getRoofEdge(path[1].vPosition, path[0].vPosition) + Vector3.up * sizeRation;
        t.Add(new BezierTrajectory(a, (a + b) / 2, b, 50, "run"));
        t.Add(new idleTrajectory(b, 8, "jump"));
        t.Add(new BezierTrajectory(b, jumpApex(b, c), c, 50, "fall"));
        t.Add(new idleTrajectory(c, 8, "land"));
    }

    private Vector3 getRoofEdge(Vector3 start, Vector3 end)
    {
        return start + ((end - start).normalized * runDistance);
    }

    private Vector3 jumpApex(Vector3 a, Vector3 b)
    {
        return ((a + b) / 2) + Vector3.up * jumpHeight;
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
