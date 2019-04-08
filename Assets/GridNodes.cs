using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodes : MonoBehaviour
{
    public int GridSizeX;
    public int GridSizeY;
    public float NodeSize;
    public float ObstacleProbability;
    public bool AllowDiagonals;

    Node[,] NodeArray;
    public List<Node> FinalPath;


    Vector2 vGridWorldSize;
    float fDistanceBetweenNodes;
    float NodeRadius;


    private void Start()
    {
        InitData();
    }

    public void InitData(){
        NodeRadius = NodeSize / 2;
        fDistanceBetweenNodes = NodeRadius / 2;
        vGridWorldSize = new Vector2(GridSizeX * NodeSize, GridSizeY * NodeSize);
        CreateGrid();
    }

    void CreateGrid()
    {
        NodeArray = new Node[GridSizeX, GridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < GridSizeX; x++){
            for (int y = 0; y < GridSizeY; y++){
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * NodeSize + NodeRadius) + Vector3.forward * (y * NodeSize + NodeRadius);
                bool Wall = (Random.Range(0.0f, 1.0f) <= ObstacleProbability) ? true : false;
                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
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

    public Vector3 GetStartPosition(){
        return NodeArray[0,0].vPosition;
    }

    public Vector3 GetEndPosition(){
        return NodeArray[GridSizeX-1, GridSizeY-1].vPosition;
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
                    }

                }


                Gizmos.DrawCube(n.vPosition, Vector3.one * (NodeSize - fDistanceBetweenNodes));//Draw the node at the position of the node.
            }
        }
    }
}
