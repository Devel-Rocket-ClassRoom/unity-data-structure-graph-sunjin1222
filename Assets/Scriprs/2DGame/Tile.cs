using UnityEngine;


public enum sides
{ 
    Bottom,
    Right,
    Left,
    Top


}


public class Tile 
{

    public int id;

    public Tile[] adjacents=new Tile[4];

    public int autoTileId;


    public bool isVisited=false;


    public void UpdateAutoTile()
    {
        autoTileId = 0;

        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i]!=null)
            {
                autoTileId |= 1 << adjacents.Length - 1-i;
            }
        }
    }
}
