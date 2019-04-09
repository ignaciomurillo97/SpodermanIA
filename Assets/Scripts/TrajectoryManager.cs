using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrajectoryManager
{
    List<Trajectory> trajectories;
    public int currTrajectory;
    public bool done;

    public TrajectoryManager()
    {
        this.done = true;
        this.currTrajectory = 0;
        this.trajectories = new List<Trajectory>();
    }

    public void setTrajectories(List<Trajectory> trajectories)
    {
        this.done = false;
        this.currTrajectory = 0;
        this.trajectories = trajectories;
    }

    public Vector3 getNextStep()
    {
        Vector3 step = trajectories[currTrajectory].nextStep();
        if (trajectories[currTrajectory].done)
        {
            if (currTrajectory >= trajectories.Count - 1)
            {
                done = true;
            }
            currTrajectory++;
        }
        return step;
    }

    public string currState ()
    {
        if (done) return "done";
        return trajectories[currTrajectory].state;
    }
}
