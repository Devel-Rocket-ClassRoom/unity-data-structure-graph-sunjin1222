using UnityEditor;
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
    public bool IsVisited=false;
    public bool IsExplored = false;
    public Map map;

    //3
    public bool CanMove => Weight!=int.MaxValue;
    public int Weight => tableWeight[autoTileId + 1];
    public static readonly int[] tableWeight =
    {
        int.MaxValue,
        1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
        2,4,int.MaxValue,1,1,1,
    };
    public Tile previousTile = null;

    public void ClrearpreviousTile()
    {
        previousTile=null;
    }
//3

    public int row; //2
    public int col; //2
    public int gCost;//2
    public int hCost;//2
    public int fCost => gCost + hCost;

    public Tile previous;

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

    //public void UpdatefowTileId()//
    //{
    //    fowId = 0;

    //    for (int i = 0; i < adjacents.Length; ++i)
    //    {
    //        if (adjacents[i] == null|| !adjacents[i].IsVisited)
    //        {
    //            fowId |= 1 << adjacents.Length - 1 - i;
    //        }
    //    }
    //}

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
