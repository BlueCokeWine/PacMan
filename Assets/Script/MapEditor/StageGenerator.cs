#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using Newtonsoft.Json;
using System.Reflection;
using System;

[ExecuteInEditMode]
public class StageGenerator : MonoBehaviour
{

	[SerializeField] GameObject defaultStagePref;
	Stage currentStage;

	public void CreateStage()
	{
		DestroyCurrentStage();
		currentStage = Instantiate(defaultStagePref).GetComponent<Stage>();
	}

	public void SaveStage()
	{
		if (currentStage == null)
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
			currentStage.name + ".prefab",
			"");

		if (path.Length != 0)
		{
			PrefabUtility.SaveAsPrefabAsset(currentStage.gameObject, path);
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

			DestroyCurrentStage();
			currentStage = Instantiate(loadStagePref).GetComponent<Stage>();
		}
	}

	public void CreatePortal()
	{
		if(currentStage == null)
		{
			return;
		}

		GameObject group = new GameObject("Warp");
		GameObject warp = new GameObject("Entrance");
		GameObject exit = new GameObject("Exit");

		group.transform.parent = currentStage.MazeGrid;
		warp.transform.parent = group.transform;
		exit.transform.parent = group.transform;

		WarpTile script = warp.AddComponent<WarpTile>();
		script.LinkDoorway(exit.transform);
		warp.AddComponent<BoxCollider>();
		warp.GetComponent<BoxCollider>().isTrigger = true;

		Texture2D tex = EditorGUIUtility.IconContent("sv_label_0").image as Texture2D;
		Type editorGUIUtilityType = typeof(EditorGUIUtility);
		BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
		object[] args = new object[] { warp, tex };
		object[] args2 = new object[] { exit, tex };
		editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
		editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args2);
	}

	void DestroyCurrentStage()
	{
		if(currentStage != null)
		{
			DestroyImmediate(currentStage.gameObject);
		}
	}

}
