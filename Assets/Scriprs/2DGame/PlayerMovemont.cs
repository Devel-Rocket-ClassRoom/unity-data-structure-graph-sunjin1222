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
    public float moveDuration = 0.2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.speed = 0;
        var findGo = GameObject.FindWithTag("Map");
        Stage = findGo.GetComponent<Stage>();

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
            }

            return;
        }

        var UP = Input.GetKeyDown(KeyCode.UpArrow);
        var Down = Input.GetKeyDown(KeyCode.DownArrow);
        var Left = Input.GetKeyDown(KeyCode.RightArrow);
        var right = Input.GetKeyDown(KeyCode.LeftArrow);
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

        startPos = transform.position;
        targetPos = Stage.GetTilePos(TileId);
        moveTime = 0f;
        isMoving = true;

        Stage.map.UpdateFog(currentTileId);
        Stage.RefreshAllTiles();
    }

    public void MoveTo(int i, int y)
    {

    }
  

}
