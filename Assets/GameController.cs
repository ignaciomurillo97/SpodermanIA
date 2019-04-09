using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GridNodes Grid;

    void Awake(){
        Grid = GetComponent<GridNodes>();
        Grid.GridSizeX = 10;
        Grid.GridSizeY = 2;
        Grid.NodeSize = 5;
        Grid.ObstacleProbability = 0.20f;
        Grid.AllowDiagonals = false;
    }

    void Start (){
        DictationScript script = GetComponent<DictationScript>();
        //script.PlayDebug();
    }

    void Update(){}

    public GridNodes GetGrid(){
        return Grid;
    }
}