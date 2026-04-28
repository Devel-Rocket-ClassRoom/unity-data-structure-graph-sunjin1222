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

    public int fowId;

    public bool isVisited=false;
    public bool CanMove=true;


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
    public void UpdateAutofowId()
    {
        fowId = 0;

        for (int i = 0; i < adjacents.Length; ++i)
        {
            if (adjacents[i] != null && adjacents[i].isVisited)
            {
                fowId |= 1 << adjacents.Length - 1 - i;
                adjacents[i].isVisited = true;
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
                UpdateAutofowId();

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
        UpdateAutofowId();


    }

}
