using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMatrix : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 gridSize;
    GameObject[][] gridOfGameObjects; 
    int gridOffset = 20;
    void Start()
    {
        gridSize = new Vector2(10, 10);
        gridOfGameObjects = new GameObject[(int)gridSize.x][];
        for (int x = 0; x < gridSize.x; x++)
        {

            gridOfGameObjects[x] = new GameObject[(int)gridSize.y];
            for (int y = 0; y < gridSize.y; y++)
            {                
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position =  new Vector3(this.transform.position.x + (gridOffset*x), this.transform.position.y, this.transform.position.z + (gridOffset*y));                
                gridOfGameObjects[x][y] = go;
            }
        
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
