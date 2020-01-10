using System.Collections;
#pragma warning disable CS0649
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stage : MonoBehaviour
{
	[SerializeField] Transform mazeGrid;
	[SerializeField] Tilemap groundTilemap;
	[SerializeField] Tilemap foodTilemap;
	[SerializeField] Tilemap wallTilemap;
	[SerializeField] Animator wallAnimator;
	[SerializeField] GameObject readyText;

	[SerializeField] Transform playerStartPlace;
	[SerializeField] List<Transform> ghostStartPlace;

	public Vector2Int Min { get; private set; }
	public Vector2Int Max { get; private set; }
	public Vector2Int Size { get; private set; }
	
	public Transform MazeGrid {
		get { return mazeGrid; }
	}
	public Transform PlayerStartPlace {
		get { return playerStartPlace; }
	}
	public List<Transform> GhostStartPlace {
		get { return ghostStartPlace; }
	}

	void Awake()
	{
		foodTilemap.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		Min = (Vector2Int)groundTilemap.cellBounds.min;
		Max = (Vector2Int)groundTilemap.cellBounds.max;
		Size = (Vector2Int)groundTilemap.cellBounds.size;
	}

	public Vector2Int GetPlayerStartPlace()
	{
		return Util.RoundToVectorInt((Vector2)playerStartPlace.position);
	}

	public List<Vector2Int> GetGhostStartPlace()
	{
		List<Vector2Int> placeList = new List<Vector2Int>();

		foreach(Transform child in ghostStartPlace)
		{
			Vector2Int position = Util.RoundToVectorInt((Vector2)child.position);
			placeList.Add(position);
		}

		return placeList;
	}

	public bool CanMove(Vector2Int place)
	{
		Vector3Int checkPlace = new Vector3Int(place.x, place.y, 0);
		if (!wallTilemap.HasTile(checkPlace) && groundTilemap.HasTile(checkPlace))
		{
			return true;

		}
		return false;
	}

	public void SetActiveReadyText(bool active)
	{
		readyText.SetActive(active);
	}

	public void SetActiveWallTwinkle(bool active)
	{
		wallAnimator.SetBool("ActiveTwinkle", active);
	}

}
