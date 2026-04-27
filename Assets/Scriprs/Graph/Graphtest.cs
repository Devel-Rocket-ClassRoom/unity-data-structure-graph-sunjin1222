using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.InputSystem;
using static Graphtest;

public class Graphtest : MonoBehaviour
{
    public enum Algorithm
    {
        DFS,
        BFS,

        DFSRecursive,

        pathFimdingBFS,

        Dijkstra,
        Astar,

    }



    public Transform uiNodeRoot;
    public List<UIGraphNode> uiNodes = new List<UIGraphNode>();
    public UIGraphNode nodePrefad;
    private Graph graph;

    public Algorithm algorithm;
    public int startId;
    public int endId;


    private void Start()
    {
        int[,] map = new int[5, 5]
            {
                { 1,-1,1,1,1},
                { 1,-1,1,1,1},
                { 1,-1,1,1,1},
                { 1,-1,1,1,1},
                { 1,1,1,1,1},
            };

        graph = new Graph();
        graph.init(map);
  
        InitUiNodes(graph);
        Search();
    }


    private void InitUiNodes(Graph graph)
    {
        foreach (var node in graph.nodes)
        {
            var uiNode = Instantiate(nodePrefad, uiNodeRoot);
            uiNode.setNode(node);
            uiNode.Reset2();
            uiNodes.Add(uiNode);


        }
    }
    private void ResetUiNodes()

    {
        foreach (var uiNode in uiNodes)
        {
            uiNode.Reset2();
        }
    }

    [ContextMenu("Search")]
    public void Search()
    {
        var search = new GraphSearch();
        search.Init(graph);

        switch (algorithm)
        {
            case Algorithm.DFS:
                search.DFS(graph.nodes[startId]);
                break;

            case Algorithm.BFS:
                search.BFS(graph.nodes[startId]);
                break;
            case Algorithm.DFSRecursive:
                search.DFSRecursive(graph.nodes[startId]);
                break;

            case Algorithm.pathFimdingBFS:
                search.pathFimdingBFS(graph.nodes[startId], graph.nodes[endId]);
                break;

            case Algorithm.Dijkstra:
               search.Dijkstra(graph.nodes[startId], graph.nodes[endId]);
                break;
            case Algorithm.Astar:
                search.Astar(graph.nodes[startId], graph.nodes[endId]);
                break;

        }

        ResetUiNodes();
        if (search.path.Count <= 1)
        {
            if (search.path.Count == 1)
            {
                var only = search.path[0];
                uiNodes[only.id].setcolor(Color.red);
            }

            return;
        }

        for (int i = 0; i < search.path.Count; i++)
        {
            var node = search.path[i];
            var color = Color.Lerp(Color.red, Color.green, (float)i / (search.path.Count - 1));
            uiNodes[node.id].setcolor(color);
        }
        

      }
}
