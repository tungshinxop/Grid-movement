using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile data")]
public class TileData : ScriptableObject
{
    public bool walkable;
    public List<TileBase> tiles;
}
