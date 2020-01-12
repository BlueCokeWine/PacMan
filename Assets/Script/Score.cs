#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
	public enum EType
	{
		Ghost,
		Fruit
	}

	const float SelfDestroyTime = 3.0f;

	[SerializeField] TextMeshPro scoreText;

	float activeTimer;

	public bool CanDeactive { get; private set; } = true;

	public void Init(EType type, int score)
	{

		CanDeactive = false;
		activeTimer = SelfDestroyTime;

		scoreText.text = score.ToString();
		switch (type)
		{
			case EType.Ghost:
				scoreText.color = Color.cyan;
				break;
			case EType.Fruit:
				scoreText.color = Color.magenta;
				break;
			default:
				scoreText.color = Color.white;
				break;
		}
	}

	public void DoUpdate()
	{
		activeTimer -= Time.deltaTime;

		if(activeTimer < 0.0f)
		{
			CanDeactive = true;
		}
	}

}
