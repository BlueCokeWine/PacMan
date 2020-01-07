using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

	public static Vector3Int RoundToVectorInt(Vector3 vector)
	{
		Vector3Int vectorInt = new Vector3Int();

		vectorInt.x = Mathf.RoundToInt(vector.x);
		vectorInt.y = Mathf.RoundToInt(vector.y);
		vectorInt.z = Mathf.RoundToInt(vector.z);

		return vectorInt;
	}

	public static Vector2Int RoundToVectorInt(Vector2 vector)
	{
		Vector2Int vectorInt = new Vector2Int();

		vectorInt.x = Mathf.RoundToInt(vector.x);
		vectorInt.y = Mathf.RoundToInt(vector.y);

		return vectorInt;
	}

}
