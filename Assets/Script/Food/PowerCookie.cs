using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCookie : Food
{
	public const int Score = 200;

	protected override void EatEvent()
	{
		var ghostList = StageManager.Instance.GhostList;

		foreach(var child in ghostList)
		{
			child.SetState(Ghost.EState.Timid);
		}

		ScoreManager.Instance.AddFoodScore(Score);
	}

}
