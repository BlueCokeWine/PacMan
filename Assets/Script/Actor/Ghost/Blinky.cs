using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
	private void Awake()
	{
		gizmoColor = Color.red;
	}

	// 플레이어의 뒤를 쫓음
	protected override void UpdateActionDecision()
	{
		Vector2Int targetPlace;

		switch (currentState)
		{
			case EState.Warp:
				if(waypointQueue.Count == 0)
				{
					if(currentState != EState.Timid) {
						currentState = EState.Normal;
					}
				}
				break;
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
				targetPlace.x += (int)StageManager.Instance.Player.CurrentDir.X;
				targetPlace.y += (int)StageManager.Instance.Player.CurrentDir.Y;
				SetTargetPlace(targetPlace);
				break;
		}
	}

}
