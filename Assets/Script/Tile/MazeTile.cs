using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EMazeTileType
{
	Ground,
	Wall,
	GhostDoor,
	GhostHome
}

public class MazeTile : Tile
{
	

	public Vector3Int LocalPlace { get; set; }

	public Vector3 WorldLocation { get; set; }

	public TileBase TileBase { get; set; }

	public string Name { get; set; }

	public EMazeTileType TileType { get; set; }

}
