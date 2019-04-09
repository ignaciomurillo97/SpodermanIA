using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GridNodes Grid;

    void Awake(){
        Grid = GetComponent<GridNodes>();
        Grid.GridSizeX = 10;
        Grid.GridSizeY = 10;
        Grid.NodeSize = 40;
        Grid.ObstacleProbability = 0.30f;
        Grid.AllowDiagonals = false;
    }

    void Start (){
        
    }

    void Update(){}

    public GridNodes GetGrid(){
        return Grid;
    }
}