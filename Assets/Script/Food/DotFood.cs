using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotFood : Food
{
	public const int Score = 100;

	void Awake()
	{
		foodType = EFood.DotFood;
	}

	protected override void EatEvent()
	{
		ScoreManager.Instance.AddFoodScore(foodType);
		gameObject.SetActive(false);
	}
}
