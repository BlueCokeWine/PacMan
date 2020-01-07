using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : Food
{
	public const int Score = 100;

	void Awake()
	{
		foodType = EFood.Cookie;
	}

	protected override void EatEvent()
	{
		ScoreManager.Instance.AddFoodScore(foodType);
		gameObject.SetActive(false);
	}
}
