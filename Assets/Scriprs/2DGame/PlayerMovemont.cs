using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovemont : MonoBehaviour
{
    private Animator animator;

    private Stage Stage;

    private int currentTileId;


    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveTime = 0f;
    public float moveDuration = 0.5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;

        var findGo = GameObject.FindWithTag("Map");
        Stage = findGo.GetComponent<Stage>();

        map = Stage.map;
    }


    private void Update()
    {
        if (isMoving)
        {
            animator.speed = 3;
            moveTime += Time.deltaTime;
            float t = moveTime / moveDuration;

            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (t >= 1f)
            {
                transform.position = targetPos;
                animator.speed = 0;
                isMoving = false;

                pathIndex++;

                if (pathIndex < currentPath.Count)
                {
                    MoveTo(currentPath[pathIndex].id);
                }
            }

            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            int targetTileId = Stage.ScreenPosToTileId(Input.mousePosition);

            Tile startTile = Stage.map.tiles[currentTileId];
            Tile targetTile = Stage.map.tiles[targetTileId];

            if (targetTile != null && targetTile.CanMove)
            {
                MoveTo(startTile, targetTile);
            }
        }
        var UP = Input.GetKeyDown(KeyCode.UpArrow);
        var Down = Input.GetKeyDown(KeyCode.DownArrow);
        var Right = Input.GetKeyDown(KeyCode.RightArrow);
        var Left = Input.GetKeyDown(KeyCode.LeftArrow);
        var direction = sides.none;
        
        if (UP)
        {
            direction= sides.Top;
        }
        else if (Down)
        {
            direction = sides.Bottom;
        }
        else if (Right)
        {
            direction = sides.Right;
        }
        else if (Left)
        {
            direction = sides.Left;
        }
        if (direction != sides.none)
        {
            var targeTile = Stage.map.tiles[currentTileId].adjacents[(int)direction];

            if (targeTile != null && targeTile.CanMove)
            {
                Tile startTile = Stage.map.tiles[currentTileId];

                MoveTo(startTile, targeTile);
            }
        }


    }


    public void MoveTo(int TileId)
    {
        currentTileId = TileId;

        startPos = transform.position;
        targetPos = Stage.GetTilePos(TileId);
        moveTime = 0f;
        isMoving = true;
     
        Stage.map.UpdateFog(currentTileId);
        Stage.RefreshAllTiles();
    }

    //public float MoveSpeed = 10f;
    //private IEnumerator CoMove()
    //{
    //    isMoving = true;

    //    var startPos=transform.position;
    //    var endPos= Stage.GetTilePos(TileId);
    //    var duration= Vector3.Distance(startPos, endPos)/ MoveSpeed;

    //    var t = 0f;

    //    while ((t >= 1f))
    //    {
    //        transform.position = Vector3.Lerp(startPos, targetPos, t);


    //    }

    //}
    private List<Tile> path = new List<Tile>();
    private Map map;
    private List<Tile> currentPath = new List<Tile>();
    private int pathIndex = 0;
    public void MoveTo(Tile start, Tile targetTile)
    {
        currentPath = FindPath(start, targetTile);
        pathIndex = 1;

        if (currentPath.Count > 1)
        {
            MoveTo(currentPath[pathIndex].id);
        }
    }




    public List<Tile> FindPath(Tile start, Tile goal)
    {
        path.Clear();

        foreach (var tile in map.tiles)
        {
            tile.previous = null;
        }

        if (start == null || goal == null)
            return path;
       

        var visited = new HashSet<Tile>();
        var pq = new PriorityQueue<Tile, int>();
        var distances = new int[map.tiles.Length];

        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = int.MaxValue;
        }
        distances[start.id] = 0;
        pq.Enqueue(start, distances[start.id] + Heuristic(start, goal));
        bool success = false;

        while (pq.Count > 0)
        {
            var currentNode = pq.Dequeue();

            if (visited.Contains(currentNode))
            {
                continue;
            }
            if (currentNode == goal)
            {
                success = true;
                break;
            }

            visited.Add(currentNode);

            foreach (var adjacent in currentNode.adjacents)
            {
                if (adjacent == null)
                    continue;

                if (!adjacent.CanMove || visited.Contains(adjacent))
                    continue;

                int newDistance = distances[currentNode.id] + 1;

                if (distances[adjacent.id] > newDistance)
                {
                    distances[adjacent.id] = newDistance;
                    adjacent.previous = currentNode;

                    int priority = newDistance + Heuristic(adjacent, goal);
                    pq.Enqueue(adjacent, priority);
                }
            }

        }
            if (!success)
            {
                return path;
            }

        Tile step = goal;
        int safety = 0;

        while (step != null)
        {
            path.Add(step);

            if (step == start)
                break;

            step = step.previous;

            safety++;
            if (safety > map.tiles.Length)
            {
                Debug.LogError("경로 무한루프!");
                break;
            }
        }

        path.Reverse();
        return path;
    
    }
    private int Heuristic(Tile a, Tile b)
    {
        int ax = a.id % map.cols;
        int ay = a.id / map.cols;

        int bx = b.id % map.cols;
        int by = b.id / map.cols;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    }

}



  


