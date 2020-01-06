using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Stage : MonoBehaviour
{
	[SerializeField]
	Tilemap foodTilemap;
	[SerializeField]
	Tilemap wallTilemap;

	[SerializeField]
	Transform playerStartPlace;
	[SerializeField]
	List<Transform> ghostStartPlace;
	
	public Transform PlayerStartPlace {
		get { return playerStartPlace; }
	}
	public List<Transform> GhostStartPlace {
		get { return ghostStartPlace; }
	}

	void Awake()
	{
		foodTilemap.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	}

	public Vector3Int GetPlayerStartPlace()
	{
		return Util.RoundToVectorInt(playerStartPlace.position);
	}

	public List<Vector3Int> GetGhostStartPlace()
	{
		List<Vector3Int> placeList = new List<Vector3Int>();

		foreach(Transform child in ghostStartPlace)
		{
			Vector3Int position = Util.RoundToVectorInt(child.position);
			placeList.Add(position);
		}

		return placeList;
	}

	public bool CanMove(Vector3Int place)
	{
		if (wallTilemap.HasTile(place))
		{
			return false;
		}

		return true;
	}

}
