using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class PowerCookieTile : Tile
{
	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		return base.StartUp(position, tilemap, go);
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/PowerCookieTile")]
	public static void CreateWaterTile()
	{
		string path = EditorUtility.SaveFilePanelInProject("Save PowerCookieTile", "New PowerCookieTile", "asset", "Save PowerCookieTile", "Assets");
		if (path == "")
		{
			return;
		}
		AssetDatabase.CreateAsset(CreateInstance<PowerCookieTile>(), path);
	}
#endif
}
