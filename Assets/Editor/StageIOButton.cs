using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageGenerator))]

public class StageIOButton : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		StageGenerator generator = (StageGenerator)target;

		if (GUILayout.Button("New Stage"))
		{
			generator.CreateStage();
		}

		if (GUILayout.Button("Stage Save"))
		{
			generator.SaveStage();
		}

		if(GUILayout.Button("Stage Load"))
		{
			generator.LoadStage();
		}

		if(GUILayout.Button("Create Warp Gate"))
		{
			generator.CreatePortal();
		}
	}

}
