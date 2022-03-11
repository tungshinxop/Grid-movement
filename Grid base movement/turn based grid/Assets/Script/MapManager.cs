using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public GameObject pref;
    public Tilemap tilemap;
    public Transform player;
    public List<TileData> tileDatas;
    public Vector3Int startingPosition;
    public float perTileMoveSpeed = 0.2f;

    private Dictionary<TileBase, TileData> dataFromTiles = new Dictionary<TileBase, TileData>();

    private BreadthFirstAlgorithm breadthFirst;

    private Vector3 currentPosition;
    List<Vector3Int> path;
    // Start is called before the first frame update
    void Start()
    {
        InitTileData();
        breadthFirst = GetComponent<BreadthFirstAlgorithm>();
        SetPlayerPos(startingPosition + tilemap.tileAnchor);
        currentPosition = player.transform.position;
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

            path = breadthFirst.GetPath(tilemap.WorldToCell(player.transform.position), cellPosition);
        
            StartCoroutine(MoveSequence(path));
        }
    }

    IEnumerator MoveSequence(List<Vector3Int> _path)
    {
        for(int i = 0; i < _path.Count; i ++)
        {
            yield return StartCoroutine(MoveRoutine(_path[i]));
        }
    }

    IEnumerator MoveRoutine(Vector3 end)
    {
        print("Move routine");
        var elapsedTime = 0f;
        while(elapsedTime < perTileMoveSpeed){
            player.transform.position = Vector3.Lerp(currentPosition ,end + tilemap.tileAnchor,elapsedTime/perTileMoveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = end + tilemap.tileAnchor;
        currentPosition = player.transform.position;
        Instantiate(pref, end + tilemap.tileAnchor, Quaternion.identity);
    }

    void SetPlayerPos(Vector3 pos)
    {
        player.position = pos;
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

