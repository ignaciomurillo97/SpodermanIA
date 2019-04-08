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


    private void Start()
    {
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
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * NodeSize + NodeRadius) + Vector3.forward * (y * NodeSize + NodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Wall = (Random.Range(0.0f, 1.0f) <= ObstacleProbability) ? false : true;
                NodeArray[x, y] = new Node(Wall, worldPoint, x, y);//Create a new node in the array.
            }
        }
    }

    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < GridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < GridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(NodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
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
                if (n.bIsWall)//If the current node is a wall node
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.yellow;//Set the color of the node
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
