using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class Stage : MonoBehaviour
{
    public GameObject tilePrefabs;

    private GameObject[] tileobjs;


    public int mapWidth = 20;
    public int mapHeight = 20;

    public Camera mainCamera;


    public PlayerMovemont PlayerPrefad;
    private PlayerMovemont Player;

    [Range(0f, 0.9f)]
    public float erodePercent = 0.5f;
    public int erodeIterations = 2;
    public float lakePercent = 0.5f;
    public float treePercent = 0.5f;
    public float hillPercent = 0.5f;
    public float mountainPercen = 0.5f;
    public float townPercent = 0.5f;
    public float monsterPercent = 0.5f;

    public Vector2 tileSize = new Vector2(16, 16);

    public Sprite[] islandSprite;

    public Sprite[] fowprite;

    public Map map;
    private Map Map => map;

    private int prevTileId = -1;

    private Vector3 FirstTilePos
    {
        get
        {
            var pos = transform.position;

            pos.x -= mapWidth * tileSize.x * 0.5f;
            pos.y += mapHeight * tileSize.y * 0.5f;

            pos.x += tileSize.x * 0.5f;
            pos.y -= tileSize.y * 0.5f;

            return pos;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetStage();
        }

        if (tileobjs != null)
        {
            int cur = ScreenPosToTileId(Input.mousePosition);
            if (prevTileId != cur)
            {
                tileobjs[cur].GetComponent<SpriteRenderer>().color = Color.green;
                if (prevTileId >= 0 && prevTileId < tileobjs.Length)
                {
                    tileobjs[prevTileId].GetComponent<SpriteRenderer>().color = Color.white;
                }
                prevTileId = cur;
            }
        }
    }

    private void ResetStage()
    {
        map = new Map();
        map.Init(mapHeight, mapWidth);
        map.CreateIsland(erodePercent, erodeIterations, lakePercent, treePercent,hillPercent, mountainPercen, townPercent, monsterPercent);

        CreateGride();
        CreatePlayer();
    }



    private void CreatePlayer()
    {
        if (Player != null)
        {
            Destroy(Player.gameObject);
        }

        Player = Instantiate(PlayerPrefad);
        Player.MoveTo(map.StartTile.id);
    }





    private void CreateGride()
    {
        if (tileobjs != null)
        {
            foreach (var tile in tileobjs)
            {
                if (tile != null)
                    Destroy(tile.gameObject);
            }
        }
   
        tileobjs = new GameObject[mapWidth * mapHeight];
        var position = Vector3.zero;

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                int tileId = i * mapWidth + j;

                var newGo = Instantiate(tilePrefabs, transform);
                newGo.transform.position =GetTilePos(tileId);

                tileobjs[tileId] = newGo;

                DecorateTile(tileId);
            }
        }
    }


    public void DecorateTile(int tileId)
    {
        var tile = map.tiles[tileId];
        var tilego = tileobjs[tileId];
        var ren = tilego.GetComponent<SpriteRenderer>();

        int index = tile.autoTileId;

        if (index <= 0 || index >= islandSprite.Length)
            ren.sprite = null;
        else
            ren.sprite = islandSprite[index];

        if (tile.fowId == 0|| tile.fowId == 1)
        {
            ren.color = Color.white; // 현재 보임
        }
        else
        {
            ren.sprite = fowprite[2]; // 완전 안개
            ren.color = Color.white;
        }
    }
    public void RefreshAllTiles()
    {
        for (int i = 0; i < tileobjs.Length; i++)
        {
            DecorateTile(i);
        }
    }

    public int ScreenPosToTileId(Vector3 screenPos)
    {
        screenPos.z = Mathf.Abs(transform.position.z - mainCamera.transform.position.z);
        return WorldPosToTileId(mainCamera.ScreenToWorldPoint(screenPos));
    }

    public int WorldPosToTileId(Vector3 worldPos)
    {
        var first = FirstTilePos;
        int x = Mathf.FloorToInt((worldPos.x - first.x) / tileSize.x + 0.5f);
        int y = Mathf.FloorToInt((first.y - worldPos.y) / tileSize.y + 0.5f);
        x = Mathf.Clamp(x, 0, mapWidth - 1);
        y = Mathf.Clamp(y, 0, mapHeight - 1);
        return y * mapWidth + x;
    }
    public Vector3 GetTilePos(int y, int x)
     => FirstTilePos + new Vector3(x * tileSize.x, -y * tileSize.y);

    public Vector3 GetTilePos(int tileId)
        => GetTilePos(tileId / mapWidth, tileId % mapWidth);

}
