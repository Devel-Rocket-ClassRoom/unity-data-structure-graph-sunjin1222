using NUnit.Framework;
using System;

using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public enum TileTypes
{ 
    Empty=-1,

    Grass=15,
    tree,
    Hill,
    Mountains,
    Towns,
    Castle,
    Monster,


}

public class Map
{

    public int rows = 0;
    public int cols = 0;


    public Tile[] coastTiles => tiles .Where(w => w.autoTileId >= 0 && w.autoTileId < (int)TileTypes.Grass) .ToArray();
    public Tile[] landTiles => tiles.Where(t=> t.autoTileId == (int)TileTypes.Grass).ToArray();

    public Tile[] tiles;


    public Tile StartTile;

    public Tile caseTile;

    public Stage stage;

    public void Init(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        

        tiles = new Tile[rows * cols];
        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i] = new Tile();
            tiles[i].id = i;

        }

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int index = r * cols + c;
       
                tiles[index].row = r;//2
                tiles[index].col = c;//2


                var adjacents = tiles[index].adjacents;

                if (r - 1 >= 0) // 위쪽
                {
                    adjacents[(int)sides.Top] = tiles[index - cols];
                }

                if (c + 1 < cols) // 오른쪽
                {
                    adjacents[(int)sides.Right] = tiles[index + 1];
                }

                if (r + 1 < rows) // 아래쪽
                {
                    adjacents[(int)sides.Bottom] = tiles[index + cols];
                }
            

                if (c - 1 >= 0) // 왼쪽
                {
                    adjacents[(int)sides.Left] = tiles[index - 1];
                }

            }

        }

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].UpdateAutoTileId();
          
            //tiles[i].UpdatefowTileId();//
        }


    }
    public void ShuffleTiles(Tile[] tiles)
    {
        for (int i = tiles.Length - 1; i > 0; --i)
        { 
            int rand= UnityEngine.Random.Range(0, i+1);
            (tiles[rand], tiles[i]) = (tiles[i], tiles[rand]);
        }
    }


    public void DecorateTiles(Tile[] tiles, float percent, TileTypes tileTypes)
    {
        ShuffleTiles(tiles);

        int total = Mathf.FloorToInt(tiles.Length * percent);

        for (int i = 0; i < total; ++i)
        {
            tiles[i].autoTileId = (int)tileTypes;

            if (tileTypes == TileTypes.Empty)
            {
                tiles[i].ClearAdjacents();
            }
        }

    }

    public void Landmark(Tile[] tiles, float percent, TileTypes tileTypes)
    {
        ShuffleTiles(tiles);

        int total = Mathf.FloorToInt(tiles.Length * percent);

      
            tiles[0].autoTileId = (int)tileTypes;

            if (tileTypes == TileTypes.Empty)
            {
                tiles[0].ClearAdjacents();
            }
        

    }
    public void UpdateFog(int playerTileId, int viewRange = 3)
    {
        foreach (var tile in tiles)
        {
            tile.IsVisited = false;
        }

        int playerX = playerTileId % cols;
        int playerY = playerTileId / cols;

        for (int y = -viewRange; y <= viewRange; y++)
        {
            for (int x = -viewRange; x <= viewRange; x++)
            {
                int checkX = playerX + x;
                int checkY = playerY + y;

                if (checkX < 0 || checkX >= cols ||checkY < 0 || checkY >= rows)
                {
                    continue;
                }

                int checkTileId = checkY * cols + checkX;
                Tile tile = tiles[checkTileId];

                tile.IsVisited = true;
                tile.IsExplored = true;
            }
        }

        foreach (var tile in tiles)
        {
            if (tile.IsVisited)
            {
                tile.fowId = 0;
            }
            else if (tile.IsExplored)
            {
                tile.fowId = 0;
            }
            else
            {
                tile.fowId = 2;
            }
        }
    }

    ////
    //public int viewRange2 = 1;
    //public void UpdateFog2(int playerTileId)
    //{
    //    int playerX = playerTileId % cols;
    //    int playerY = playerTileId / cols;

    //    for (int i = -viewRange2; i <= viewRange2; i++)
    //    {
    //        for (int j = -viewRange2; j <= viewRange2; j++)
    //        {
    //            int x = playerX + j;
    //            int y = playerY + i;
    //            if (x < 0 || x >= stage.mapWidth || y < 0 || y >= stage.mapHeight)
    //                continue;

    //            int id=y* stage.mapWidth + x;
    //            tiles[i].IsVisited = true;
    //            stage.DecorateTile(id);

    //        }
    //    }

    //     var Range= viewRange2+1;
    //    for (int i = -Range; i <= Range; i++)
    //    {
    //        for (int j = -Range; j <= Range; j++)
    //        {
    //            if (i == -Range || i == Range || j == Range || j == -Range)
    //            {
    //                int x = playerX + j;
    //                int y = playerY + i;
    //                if (x < 0 || x >= stage.mapWidth || y < 0 || y >= stage.mapHeight)
    //                    continue;
    //            }
    //        }
    //    }


    //}//

    ////3
    //private int Heuristic(Tile a, Tile b)
    //{
    //    int ax = a.id % cols;
    //    int ay = a.id / cols;

    //    int bx = b.id % cols;
    //    int by = b.id / cols;

    //    return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by);
    //}

    //   public List<Tile> FindPath(Tile start, Tile goal)
    //{
    //    List<Tile> path = new List<Tile>();
    //    path.Clear();

    //    foreach (var tile in tiles)
    //    {
    //        tile.ClearAdjacents();
    //    }

    //    if (start == null || goal == null)
    //        return path;


    //    var visited = new HashSet<Tile>();
    //    var pq = new PriorityQueue<Tile, int>();
    //    var distances = new int[tiles.Length];

    //    for (int i = 0; i < distances.Length; i++)
    //    {
    //        distances[i] = int.MaxValue;
    //    }
    //    distances[start.id] = 0;
    //    pq.Enqueue(start, distances[start.id] + Heuristic(start, goal));
    //    bool success = false;

    //    while (pq.Count > 0)
    //    {
    //        var currentNode = pq.Dequeue();

    //        if (visited.Contains(currentNode))
    //        {
    //            continue;
    //        }
    //        if (currentNode == goal)
    //        {
    //            success = true;
    //            break;
    //        }

    //        visited.Add(currentNode);

    //        foreach (var adjacent in currentNode.adjacents)
    //        {
    //            if (adjacent == null)
    //                continue;

    //            if (!adjacent.CanMove || visited.Contains(adjacent))
    //                continue;

    //            int newDistance = distances[currentNode.id] + 1;

    //            if (distances[adjacent.id] > newDistance)
    //            {
    //                distances[adjacent.id] = newDistance;
    //                adjacent.previous = currentNode;

    //                int priority = newDistance + Heuristic(adjacent, goal);
    //                pq.Enqueue(adjacent, priority);
    //            }
    //        }

    //    }
    //    if (!success)
    //    {
    //        return path;
    //    }

    //    Tile step = goal;
    //    int safety = 0;

    //    while (step != null)
    //    {
    //        path.Add(step);

    //        if (step == start)
    //            break;

    //        step = step.previous;

    //        safety++;
    //        if (safety > tiles.Length)
    //        {
    //            Debug.LogError("경로 무한루프!");
    //            break;
    //        }
    //        path.Reverse();
    //    }

 
    //    return path;

    //}
    //33
    public bool CreateIsland(float erodePercent, int erodeIterations, float lakePercent, float treePercent, float hillPercent, float mountainPercent, float townPercent, float monsterPercent)
    {
        
            for (int i = 0; i < erodeIterations; ++i)
            {
                DecorateTiles(coastTiles, erodePercent, TileTypes.Empty);
            }

            DecorateTiles(landTiles, lakePercent, TileTypes.Empty);
            DecorateTiles(landTiles, treePercent, TileTypes.tree);
            DecorateTiles(landTiles, hillPercent, TileTypes.Hill);
            DecorateTiles(landTiles, mountainPercent, TileTypes.Mountains);
            DecorateTiles(landTiles, monsterPercent, TileTypes.Monster);
            DecorateTiles(landTiles, townPercent, TileTypes.Towns);

            var Towns=tiles.Where(x=> x.autoTileId==(int)TileTypes.Towns).ToArray();
            ShuffleTiles(Towns);

            StartTile = Towns[0];
            caseTile = Towns[1];
            caseTile.autoTileId = (int)TileTypes.Castle;

        ////3
        //var path = FindPath(start,goal)
        //    //3
            return true;
        }
    }
