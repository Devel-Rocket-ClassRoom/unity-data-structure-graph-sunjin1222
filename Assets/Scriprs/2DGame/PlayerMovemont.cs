using UnityEngine;

public class PlayerMovemont : MonoBehaviour
{
    private Animator animator;

    private Stage Stage;

    private int currentTileId;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.speed = 0;
        var findGo = GameObject.FindWithTag("Map");
        Stage = findGo.GetComponent<Stage>();

    }


    private void Update()
    {
        var UP = Input.GetKeyDown(KeyCode.UpArrow);
        var Down = Input.GetKeyDown(KeyCode.DownArrow);
        var right = Input.GetKeyDown(KeyCode.RightArrow);
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
        else if (right)
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
                MoveTo(targeTile.id);
            }

        }


    }

    public void MoveTo(int TileId)
    {
        currentTileId = TileId;
        transform.position = Stage.GetTilePos(TileId);

    }

    public void MoveTo(int i, int y)
    { 
    
    }

}
