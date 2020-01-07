using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class CookieTile : Tile
{
	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		return base.StartUp(position, tilemap, go);
	}

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Tiles/CookieTile")]
	public static void CreateWaterTile()
	{
		string path = EditorUtility.SaveFilePanelInProject("Save CookieTile", "New CookieTile", "asset", "Save CookieTile", "Assets");
		if (path == "")
		{
			return;
		}
		AssetDatabase.CreateAsset(CreateInstance<CookieTile>(), path);
	}
#endif
}
