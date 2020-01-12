using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : Food
{
	public const int Score = 10;

	protected override void EatEvent()
	{
		ScoreManager.Instance.AddScore(Score);
		AudioManager.Instance.PlayEatCookieSound();
	}
}
