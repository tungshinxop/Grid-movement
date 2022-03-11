using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public GameObject pref;
    public Tilemap tilemap;
    public Actor player;
    public List<TileData> tileDatas;
    public Vector3Int startingPosition;
    public float perTileMoveSpeed = 0.2f;

    private Dictionary<TileBase, TileData> dataFromTiles = new Dictionary<TileBase, TileData>();

    private BreadthFirstAlgorithm breadthFirst;

    void Start()
    {
        InitTileData();
        breadthFirst = GetComponent<BreadthFirstAlgorithm>();
        player.UpdatePosition(player.transform.position + tilemap.tileAnchor);
    }

    void InitTileData(){
        //Loop through all tile data for the map
        foreach(var data in tileDatas)
        {
            //Loop through each tile of the tile data
            foreach(var tile in data.tiles)
            {
                //Assign the key value pair to the each tile
                dataFromTiles.Add(tile, data);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //print("Mouse pressed");
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cellPosition = tilemap.WorldToCell(mousePosition);
            TileBase clickedTile = tilemap.GetTile(cellPosition);
            print(cellPosition);

            if(!WalkableTile(cellPosition) && IsTile(cellPosition)){
                print("INVALID CHOOSE");
                return;
            }

            var path = breadthFirst.GetPath(tilemap.WorldToCell(player.transform.position), cellPosition);
        
            StartCoroutine(player.MoveSequence(path));
        }
    }

    public bool WalkableTile(Vector3Int pos){
        var tile = GetTileFromPosition(pos);
        return dataFromTiles[tile].walkable;
    }

    public bool IsTile(Vector3Int pos){
        return tilemap.GetTile(pos) != null;
    }

    public TileData GetTileData(TileBase tile){
        return dataFromTiles[tile];
    }

    public TileBase GetTileFromPosition(Vector3Int pos){
        return tilemap.GetTile(pos);
    }
}

