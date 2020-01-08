using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
	// 플레이어의 뒤를 쫓음
	private void Awake()
	{
		gizmoColor = Color.red;
	}

	protected override void UpdateActionDecision()
	{
		Vector2Int targetPlace;

		switch (currentState)
		{
			case EState.Timid:
				if (waypointQueue.Count == 0)
				{
					targetPlace = FindTimidRunPlace(new Direction(EDirX.Right, EDirY.Up));
					SetTargetPlace(targetPlace);
				}
				break;
			case EState.GoHome:
				if (waypointQueue.Count == 0)
				{
					SetTargetPlace(homePlace);
				}
				break;
			default:
				targetPlace = StageManager.Instance.Player.CurrentPlace;
				SetTargetPlace(targetPlace);
				break;
		}
	}

}
