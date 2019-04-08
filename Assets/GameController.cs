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
        Grid.ObstacleProbability = 0.1f;
        Grid.AllowDiagonals = false;
    }

    void Start (){
        
    }

    void Update(){
        if (Random.Range(0.0f, 1.0f) <= 0.1){
            Grid.GridSizeX += 1;
            Grid.GridSizeY += 1;
            Grid.InitData();
        }
    }

    public GridNodes GetGrid(){
        return Grid;
    }
}