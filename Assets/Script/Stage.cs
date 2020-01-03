using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
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

	public Vector3Int GetPlayerStartPlace()
	{
		return RoundToVectorInt(playerStartPlace.position);
	}

	public List<Vector3Int> GetGhostStartPlace()
	{
		
	}

	public Vector3Int RoundToVectorInt(Vector3 vector)
	{
		Vector3Int vectorInt = new Vector3Int();

		vectorInt.x = Mathf.RoundToInt(vector.x);
		vectorInt.y = Mathf.RoundToInt(vector.y);
		vectorInt.z = Mathf.RoundToInt(vector.z);

		return vectorInt;
	}

}
