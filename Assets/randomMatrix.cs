using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class randomMatrix : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 gridSize;
    GameObject[][] buildingGrid; 
    Vector3[][] positionGrid;
    public GameObject building1;
    public GameObject building2;
    int gridOffset = 5;
    int listSize = 4;
    float scale = 0.1f;
    void Start()
    {

        GameObject[] buildingList = {building2};                
        gridSize = new Vector2(listSize, listSize);
        buildingGrid = new GameObject[(int)gridSize.x][];
        positionGrid = new Vector3[(int)gridSize.x][];
        for (int x = 0; x < gridSize.x; x++)
        {
            buildingGrid[x] = new GameObject[(int)gridSize.y];
            positionGrid[x] = new Vector3[(int)gridSize.y];
            for (int y = 0; y < gridSize.y; y++)
            {                                
                System.Random rand = new System.Random();  
                int index = rand.Next(buildingList.Length);  
                Vector3 pos =  new Vector3(this.transform.position.x + (gridOffset*x), this.transform.position.y, this.transform.position.z + (gridOffset*y));                
                GameObject building = Instantiate(building2, pos, Quaternion.identity);
                building.transform.localScale = new Vector3(scale, scale, scale);
                Debug.Log(pos);
                buildingGrid[x][y] = building;
                positionGrid[x][y] = pos;
            }
        
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
