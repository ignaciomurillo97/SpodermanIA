using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GridNodes Grid;

    void Awake(){
        Grid = GetComponent<GridNodes>();
        Grid.GridSizeX = 1;
        Grid.GridSizeY = 1;
        Grid.NodeSize = 2;
        Grid.ObstacleProbability = 0.0f;
        Grid.AllowDiagonals = false;
    }

    void Start (){
        
    }

    void Update(){}

    public GridNodes GetGrid(){
        return Grid;
    }
}