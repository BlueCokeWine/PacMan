using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class DotFoodTile : Tile
{
	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		return base.StartUp(position, tilemap, go);
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/DotFoodTile")]
	public static void CreateWaterTile()
	{
		string path = EditorUtility.SaveFilePanelInProject("Save DotFoodTile", "New DotFoodTile", "asset", "Save DotFoodTile", "Assets");
		if (path == "")
		{
			return;
		}
		AssetDatabase.CreateAsset(CreateInstance<DotFoodTile>(), path);
	}

#endif
}
