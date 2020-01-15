using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : Ghost
{
	const float PrepareTime = 0.0f;

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
			case EState.Prepare:
				break;
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
				float eatenFoodRatio = StageManager.Instance.GetEatenFoodRatio();
				float dynamicMoveSpeed = Mathf.Lerp(moveSpeed / 2, moveSpeed, eatenFoodRatio);
				moveHandler.MoveSpeed = dynamicMoveSpeed;

				targetPlace = StageManager.Instance.Player.CurrentPlace;
				targetPlace.x += (int)StageManager.Instance.Player.CurrentDir.X;
				targetPlace.y += (int)StageManager.Instance.Player.CurrentDir.Y;
				SetTargetPlace(targetPlace);
				break;
		}
	}

	protected override void PrepareAtHome()
	{
		StartCoroutine(StartPrepareTime(PrepareTime));
	}
}
