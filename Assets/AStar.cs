using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    GridNodes GridReference;

    private void Awake()
    {
        GridReference = GetComponent<GridNodes>();
    }

    public List<Node> ExecuteAlgorithm()
    {
        List<Node> FinalPathList = FindPath(GridReference.GetStartPosition(), GridReference.GetEndPosition());
        return FinalPathList;
    }

    List<Node> FindPath(Node StartNode, Node TargetNode)
    {
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();
        List<Node> FinalPathList = new List<Node>();

        OpenList.Add(StartNode);
        while(OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++){
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode){
                FinalPathList = GetFinalPath(StartNode, TargetNode);
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode)){
                if (NeighborNode.IsObstacle || ClosedList.Contains(NeighborNode)){
                    continue;
                }

                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);
                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode)){
                    NeighborNode.igCost = MoveCost;
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if(!OpenList.Contains(NeighborNode)){
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
        
        return FinalPathList;
    }



    List<Node> GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        while(CurrentNode != a_StartingNode){
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        FinalPath.Reverse();

        GridReference.FinalPath = FinalPath;
        return FinalPath;

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.iGridX - a_nodeB.iGridX);
        int iy = Mathf.Abs(a_nodeA.iGridY - a_nodeB.iGridY);

        return ix + iy;
    }
}
