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

		GuiLine();
		EditorGUILayout.LabelField("Stage IO:");
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

		GuiLine();
		EditorGUILayout.LabelField("Stage Object Generator:");
		if (GUILayout.Button("Create Warp Gate"))
		{
			generator.CreatePortal();
		}
		GuiLine();

		EditorGUILayout.LabelField("Stage List Generator:");
		var serializedObject = new SerializedObject(target);
		var property = serializedObject.FindProperty("stageList");
		serializedObject.Update();
		EditorGUILayout.PropertyField(property, true);
		serializedObject.ApplyModifiedProperties();

		if (GUILayout.Button("Create Stage JSON"))
		{
			generator.CreateStageListJson();
		}
		GuiLine();

	}

	void GuiLine(int i_height = 1)
	{
		Rect rect = EditorGUILayout.GetControlRect(false, i_height);
		rect.height = i_height;
		EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
	}

}
