using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinky : Ghost
{
	const int PredictionWeight = 3;

	private void Awake()
	{
		gizmoColor.r = 247f / 255f;
		gizmoColor.g = 171f / 255f;
		gizmoColor.b = 166f / 255f;
		gizmoColor.a = 1f;
	}

	protected override void UpdateActionDecision()
	{
		Vector2Int targetPlace;

		switch (currentState)
		{
			case EState.Normal:
				targetPlace = FindPredictionPlace();
				SetTargetPlace(targetPlace);

				if (waypointQueue.Count < PredictionWeight)
				{
					currentState = EState.Tracking;
					targetPlace = StageManager.Instance.Player.CurrentPlace;
					SetTargetPlace(targetPlace);
				}
				break;
			case EState.Tracking:
				targetPlace = StageManager.Instance.Player.CurrentPlace;
				SetTargetPlace(targetPlace);

				if(waypointQueue.Count > PredictionWeight * 2)
				{
					currentState = EState.Normal;
					targetPlace = FindPredictionPlace();
					SetTargetPlace(targetPlace);
				}
				break;
			case EState.Timid:
				if (waypointQueue.Count == 0)
				{
					targetPlace = FindTimidRunPlace(new Direction(EDirX.Left, EDirY.Up));
					SetTargetPlace(targetPlace);
				}
				break;
			case EState.GoHome:
				if (waypointQueue.Count == 0)
				{
					SetTargetPlace(homePlace);
				}
				break;
		}
	}

	Vector2Int FindPredictionPlace()
	{
		Vector2Int playerPlace = StageManager.Instance.Player.CurrentPlace;
		Direction playerDirection = StageManager.Instance.Player.CurrentDir;
		Vector2Int predictionPlace = playerPlace;

		for (int i = 1; i < PredictionWeight + 1; i++)
		{
			Vector2Int checkPlace = playerPlace;
			checkPlace.x += (int)playerDirection.X * i;
			checkPlace.y += (int)playerDirection.Y * i;
			if (StageManager.Instance.CanMovePlace(checkPlace))
			{
				predictionPlace = checkPlace;
			}
			else
			{
				break;
			}
		}

		return predictionPlace;
	}

}
