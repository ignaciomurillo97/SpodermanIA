using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodes : MonoBehaviour
{
    public int GridSizeX;
    public int GridSizeY;
    public float NodeSize;
    public float ObstacleProbability;
    Node[,] NodeArray;
    public List<Node> FinalPath;
    Vector2 vGridWorldSize;
    float fDistanceBetweenNodes;
    float NodeRadius;
    
    //Building Assets
    
    GameObject[][] buildingGrid; 
    GameObject[] backgrounds;
    List<GameObject> pathList = new List<GameObject>();
    GameObject[] obstacleList;
    float distanceToBackground;
    float  backgroundNorthScale = 0.005f;
    public GameObject backgroundNorth;
    public GameObject pathStart;
    public GameObject pathEnd;
    public GameObject pathProgress;
    public GameObject building1;
    public GameObject building2;
    public GameObject building3;
    public GameObject building4;
    public GameObject building5;
    public GameObject building6;
    public GameObject building7;
    public GameObject building8;
    public GameObject building9;
    public GameObject building10;
    public GameObject building11;
    public GameObject building12;
    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;
    public GameObject obstacle4;
    float building1Scale = 0.2f;
    
    float obstacle1Scale = 0.083f;
    
    public bool AllowDiagonals;

    public Node StartPosition;
    public Node TargetPosition;

    public int StartPositionX;
    public int StartPositionY;
    public int TargetPositionX;
    public int TargetPositionY;


    private void Start()
    {
        // StartPositionX = Random.Range(0, GridSizeX);
        // StartPositionY = Random.Range(0, GridSizeY);

        // TargetPositionX = StartPositionX;
        // TargetPositionY = StartPositionY;

        StartPositionX = 0;
        StartPositionY = 0;

        TargetPositionX = GridSizeX - 1;
        TargetPositionY = GridSizeY - 1;

        InitData();
    }

    public void InitData(){        
        NodeRadius = NodeSize / 2;
        fDistanceBetweenNodes = NodeRadius / 2;
        distanceToBackground = fDistanceBetweenNodes*3.5f ;
        vGridWorldSize = new Vector2(GridSizeX * NodeSize, GridSizeY * NodeSize);
        CreateGrid();
    }
    void clearBuildingGrid(){
        if (buildingGrid != null){
            if (buildingGrid[0] != null){
                for (int i = 0; i < buildingGrid.GetLength(0); i++){
                    for (int j = 0; j < buildingGrid[0].GetLength(0); j++){
                        Destroy(buildingGrid[i][j]);
                    }            
                }
            }            
        }                
        if (backgrounds != null){
            for(int i =0; i<backgrounds.GetLength(0); i++){
             if (backgrounds[i] != null){
                 Destroy(backgrounds[i]);
             }   
            }            
        }        
    }

    void CreateGrid()
    {        
        clearBuildingGrid(); 
        GameObject[] buildingList = {building1, building2, building3, building4, building5, building6, building7, building8, building9, building10, building11, building12};
        GameObject[] obstacleList = {obstacle1,obstacle2, obstacle3,obstacle4};
        float[] buildingScaleList = {building1Scale};
        float[] obstacleScaleList = {obstacle1Scale};
        buildingGrid = new GameObject[(int)GridSizeX][];
        NodeArray = new Node[GridSizeX, GridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < GridSizeX; x++){
            buildingGrid[x] = new GameObject[(int)GridSizeY];
            for (int y = 0; y < GridSizeY; y++){
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * NodeSize + NodeRadius) + Vector3.forward * (y * NodeSize + NodeRadius);
                bool Wall = (Random.Range(0.0f, 1.0f) <= ObstacleProbability) ? true : false;
                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);
                if (NodeArray[x, y].IsObstacle){
                    int index = Random.Range(0, obstacleList.GetLength(0));
                    Vector3 obstaclePos = new Vector3(worldPoint.x, worldPoint.y+(NodeSize*0.5f), worldPoint.z);
                    GameObject obstacle = Instantiate(obstacleList[index], obstaclePos, Quaternion.identity);
                    buildingGrid[x][y] = obstacle;                                        
                    obstacle.transform.Rotate(0,0,0,Space.Self);                    
                }
                else{
                    int index = Random.Range(0, buildingList.GetLength(0));
                    GameObject building = Instantiate(buildingList[index], worldPoint, Quaternion.identity);
                    buildingGrid[x][y] = building;
                    building.transform.localScale = new Vector3(NodeSize/10, NodeSize/10, NodeSize/10);                                                        
                    if (x%2 == 0){
                        building.transform.Rotate(0,180,0,Space.Self);
                    }
                }                
            }
        }
        createBackgrounds();
    }

    void renderStart(int x, int y){        
        float yOffset = NodeSize*0.5f;
        Vector3 pos = new Vector3(NodeArray[x, y].vPosition.x, NodeArray[x, y].vPosition.y+yOffset, NodeArray[x, y].vPosition.x.z);
        GameObject pathIndicator = Instantiate(pathStart, pos, Quaternion.identity);                
        pathList.Add(pathIndicator);
    }

    void renderTarget(int x, int y){        
        float yOffset = NodeSize*0.5f;
        Vector3 pos = new Vector3(NodeArray[x, y].vPosition.x, NodeArray[x, y].vPosition.y+yOffset, NodeArray[x, y].vPosition.x.z);
        GameObject pathIndicator = Instantiate(pathEnd, pos, Quaternion.identity);                                
        pathList.Add(pathIndicator);
    }

    void renderPath(int x, int y){        
        float yOffset = NodeSize*0.5f;
        Vector3 pos = new Vector3(NodeArray[x, y].vPosition.x, NodeArray[x, y].vPosition.y+yOffset, NodeArray[x, y].vPosition.x.z);
        GameObject pathIndicator = Instantiate(pathProgress, pos, Quaternion.identity);                                
        pathList.Add(pathIndicator);
    }

     void createBackgrounds(){        
        backgrounds = new GameObject[8];
        if (NodeArray[0,0] != null){        
            distanceToBackground *= (GridSizeX+GridSizeY/2)*22;
            Vector3 pos1 = new Vector3(NodeArray[0,0].vPosition.x - distanceToBackground,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z );
            Vector3 pos2 = new Vector3(NodeArray[GridSizeX-1,0].vPosition.x + distanceToBackground,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z );            
            Vector3 pos5 = new Vector3(NodeArray[0,0].vPosition.x - distanceToBackground,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z );
            Vector3 pos6 = new Vector3(NodeArray[0,0].vPosition.x - distanceToBackground,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z );
            Vector3 pos7 = new Vector3(NodeArray[GridSizeX-1,0].vPosition.x + distanceToBackground,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z );
            Vector3 pos8 = new Vector3(NodeArray[GridSizeX-1,0].vPosition.x + distanceToBackground,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z );            
            Vector3 pos3 = new Vector3(NodeArray[GridSizeX-1,GridSizeY-1].vPosition.x,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[GridSizeX-1,GridSizeY-1].vPosition.z + distanceToBackground );
            Vector3 pos4 = new Vector3(NodeArray[0,0].vPosition.x,NodeArray[0,0].vPosition.y+(vGridWorldSize.x*0.0420f), NodeArray[0,0].vPosition.z - distanceToBackground );
            if(GridSizeY > 0){                
                float z1 = NodeArray[0,0].vPosition.z;
                float z2 = NodeArray[0,GridSizeY-1].vPosition.z;
                float x1 = NodeArray[0,GridSizeY-1].vPosition.x;
                float x2 = NodeArray[GridSizeX-1,GridSizeY-1].vPosition.x;
                pos1.z = (z1 + z2)/2  - (vGridWorldSize.y*0.5f) ;
                pos2.z = (z1 + z2)/2  - (vGridWorldSize.y*0.5f) ;
                pos5.z = z2  + (vGridWorldSize.y*0.5f) ;
                pos6.z = z1  - (vGridWorldSize.y*0.5f) ;
                pos7.z = z1  - (vGridWorldSize.y*0.5f) ;
                pos8.z = z2  + (vGridWorldSize.y*0.5f) ;
                pos3.x = (x1 + x2)/2  - (vGridWorldSize.x*0.5f) ;
                pos4.x = (x1 + x2)/2  - (vGridWorldSize.x*0.5f) ;
                
            }
            
            GameObject backgroundNorth1 = Instantiate(backgroundNorth, pos1, Quaternion.identity);
            GameObject backgroundNorth2 = Instantiate(backgroundNorth, pos5, Quaternion.identity);  
            GameObject backgroundNorth3 = Instantiate(backgroundNorth, pos6, Quaternion.identity);  
            GameObject backgroundSouth1 = Instantiate(backgroundNorth, pos2, Quaternion.identity);  
            GameObject backgroundSouth2 = Instantiate(backgroundNorth, pos7, Quaternion.identity);  
            GameObject backgroundSouth3 = Instantiate(backgroundNorth, pos8, Quaternion.identity);  
            GameObject backgroundEast1 = Instantiate(backgroundNorth, pos3, Quaternion.identity);  
            GameObject backgroundWest1 = Instantiate(backgroundNorth, pos4, Quaternion.identity);  
            float bgScale = backgroundNorthScale*(GridSizeX+GridSizeY/2)*16;           
            //float bgScale = backgroundNorthScale*GridSizeX;            
            backgroundNorth1.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundNorth2.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundNorth3.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundSouth1.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundSouth2.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundSouth3.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            //bgScale = backgroundNorthScale*GridSizeY;          
            backgroundEast1.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundWest1.transform.localScale = new Vector3(NodeSize*bgScale,NodeSize*bgScale,NodeSize*bgScale);
            backgroundEast1.transform.Rotate(0,90,0,Space.Self);
            backgroundWest1.transform.Rotate(0,90,0,Space.Self);
            backgrounds[0] = backgroundNorth1;
            backgrounds[1] = backgroundSouth1;
            backgrounds[2] = backgroundEast1;
            backgrounds[3] = backgroundWest1;
            backgrounds[4] = backgroundNorth2;
            backgrounds[5] = backgroundNorth3;
            backgrounds[5] = backgroundSouth2;
            backgrounds[5] = backgroundSouth3;
        }
    }

    public Node GetStartPosition()
    {
        return NodeArray[StartPositionX, StartPositionY];
    }

    public Node GetEndPosition()
    {
        return NodeArray[TargetPositionX, TargetPositionY];
    }

    public bool CheckBorders(int x, int y){
        if (x >= 0 && x < GridSizeX){
            if (y >= 0 && y < GridSizeY){
                return true;
            }
        }

        return false;
    }

    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();
        int icheckX, icheckY;
        Node RightNode = null, LeftNode = null, TopNode = null, BottomNode = null;

        //Check the right side of the current node.
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (CheckBorders(icheckX, icheckY)){
            RightNode = NodeArray[icheckX, icheckY];
            NeighborList.Add(RightNode);
        }
        
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (CheckBorders(icheckX, icheckY)){
            LeftNode = NodeArray[icheckX, icheckY];
            NeighborList.Add(LeftNode);
        }

        //Check the Top side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (CheckBorders(icheckX, icheckY)){
            TopNode = NodeArray[icheckX, icheckY];
            NeighborList.Add(TopNode);
        }

        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (CheckBorders(icheckX, icheckY)){
            BottomNode = NodeArray[icheckX, icheckY];
            NeighborList.Add(BottomNode);
        }

        if (AllowDiagonals){
            //Check the Top Right side of the current node.
            icheckX = a_NeighborNode.iGridX + 1;
            icheckY = a_NeighborNode.iGridY + 1;
            if (CheckBorders(icheckX, icheckY)){
                if (!(TopNode.IsObstacle && RightNode.IsObstacle)){
                    NeighborList.Add(NodeArray[icheckX, icheckY]);
                }
            }

            //Check the Top Left side of the current node.
            icheckX = a_NeighborNode.iGridX - 1;
            icheckY = a_NeighborNode.iGridY + 1;
            if (CheckBorders(icheckX, icheckY)){
                if (!(TopNode.IsObstacle && LeftNode.IsObstacle)){
                    NeighborList.Add(NodeArray[icheckX, icheckY]);
                }
            }

            //Check the Bottom Right side of the current node.
            icheckX = a_NeighborNode.iGridX + 1;
            icheckY = a_NeighborNode.iGridY - 1;
            if (CheckBorders(icheckX, icheckY)){
                if (!(BottomNode.IsObstacle && RightNode.IsObstacle)){
                    NeighborList.Add(NodeArray[icheckX, icheckY]);
                }
            }

            //Check the Bottom Left side of the current node.
            icheckX = a_NeighborNode.iGridX - 1;
            icheckY = a_NeighborNode.iGridY - 1;
            if (CheckBorders(icheckX, icheckY)){
                if (!(BottomNode.IsObstacle && LeftNode.IsObstacle)){
                    NeighborList.Add(NodeArray[icheckX, icheckY]);
                }
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((GridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((GridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }


    //Function that draws the wireframe
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (NodeArray != null)//If the grid is not empty
        {
            foreach (Node n in NodeArray)//Loop through every node in the grid
            {
                if (n.IsObstacle)//If the current node is a wall node
                {
                    Gizmos.color = Color.yellow;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }


                if (FinalPath != null)//If the final path is not empty
                {
                    if (FinalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                        Gizmos.DrawCube(n.vPosition, Vector3.one * (NodeSize - fDistanceBetweenNodes));//Draw the node at the position of the node.
                    }

                }


                //Gizmos.DrawCube(n.vPosition, Vector3.one * (NodeSize - fDistanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
}
