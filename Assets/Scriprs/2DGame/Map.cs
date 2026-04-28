using System.Linq;
using UnityEditor;
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
            tiles[i].UpdateAutofowId();
        }


    }
    public void ShuffleTiles(Tile[] tiles)
    {
        for (int i = tiles.Length - 1; i > 0; --i)
        { 
            int rand=Random.Range(0, i+1);
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

            return true;
        }
    }
