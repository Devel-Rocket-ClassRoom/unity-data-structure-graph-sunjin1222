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

    public Tile[] tiles;

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

                if (r - 1 >= 0)//맨 위쪽
                {
                    adjacents[(int)sides.Top] = tiles[index - cols];
                }

                if (c + 1 < cols)//맨 오른쪽
                {
                    adjacents[(int)sides.Right] = tiles[index + 1];

                }

                if (r + 1 < rows)//맨 아래쪽
                {
                    adjacents[(int)sides.Bottom] = tiles[index + cols];
                }

                if (c - 1 >= cols)//맨 왼쪽
                {
                    adjacents[(int)sides.Left] = tiles[index - 1];
                }

            }

        }
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].UpdateAutoTile();
        }

    }
}
