using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int iGridX;
    public int iGridY;

    public bool IsObstacle;
    public Vector3 vPosition;

    public Node ParentNode;

    public int igCost;
    public int ihCost;

    public int FCost { get { return igCost + ihCost; } }

    public Node(bool a_IsObstacle, Vector3 a_vPos, int a_igridX, int a_igridY)//Constructor
    {
        IsObstacle = a_IsObstacle;
        vPosition = a_vPos;
        iGridX = a_igridX;
        iGridY = a_igridY;
    }

}
