using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Newtonsoft.Json;
public class StageGenerator : MonoBehaviour
{

	[SerializeField]
	GameObject defaultStagePref;

	public void CreateStage()
	{
		Instantiate(defaultStagePref);
	}

	public void SaveStage()
	{
		GameObject stage = Selection.activeObject as GameObject;
		if (stage == null)
		{
			EditorUtility.DisplayDialog(
				"Select Stage Game Object",
				"You Must Select a Stage first!",
				"Ok");
			return;
		}

		var path = EditorUtility.SaveFilePanel(
			"Save Stage as Prefab",
			"",
			stage.name + ".prefab",
			"");

		if (path.Length != 0)
		{
			PrefabUtility.SaveAsPrefabAsset(stage, path);
		}
	}

	public void LoadStage()
	{
		var path = EditorUtility.OpenFilePanel(
			"Load Stage Prefab",
			Application.dataPath + "/Assets",
			"prefab"
			);

		if (path.Length != 0)
		{
			GameObject loadStagePref = PrefabUtility.LoadPrefabContents(path);
			Instantiate(loadStagePref);
		}

	}

}
