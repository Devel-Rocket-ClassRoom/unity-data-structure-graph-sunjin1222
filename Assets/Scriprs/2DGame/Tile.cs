using UnityEngine;


public enum sides
{

    none = -1,
    Bottom,

    Right,
    Left,
    Top,
}


public class Tile 
{

    public int id;

    public Tile[] adjacents=new Tile[4];

    public int autoTileId;
    public bool CanMove = true;


    public int fowId;
    public bool IsVisited=false;
    public bool IsExplored = false;
    public Map map;
 


    public void UpdateAutoTileId()
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
 

    public void RemoveAdjacent(Tile tile)
    {
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null)
            {
                continue;
            }
            if (adjacents[i].id == tile.id)
            {
                adjacents[i] = null;
                UpdateAutoTileId();
            
                break;
            }
        }
    }
    public void ClearAdjacents()
    {
        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] == null)
            { 
                continue;
            }
            adjacents[i].RemoveAdjacent(this);
            adjacents[i] = null;
        }
        UpdateAutoTileId();



    }

}
