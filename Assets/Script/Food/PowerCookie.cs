using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCookie : Food
{
	public const int Score = 50;

	protected override void EatEvent()
	{
		var ghostList = StageManager.Instance.GhostList;

		foreach(var child in ghostList)
		{
			child.SetState(Ghost.EState.Timid);
		}

		ScoreManager.Instance.AddScore(Score);
		AudioManager.Instance.SetGhostSoundClip(ESfxId.EatPowerCookie);
	}

}
