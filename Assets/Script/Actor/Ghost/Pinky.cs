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

	protected override void ActiveTracking()
	{
		switch (currentStat)
		{
			case EStat.Normal:
				PredictionTrack();

				if (waypointQueue.Count < PredictionWeight)
				{
					currentStat = EStat.Tracking;
					DirectTrack();
				}
				break;
			case EStat.Tracking:
				DirectTrack();

				if(waypointQueue.Count > PredictionWeight * 2)
				{
					currentStat = EStat.Normal;
					PredictionTrack();
				}
				break;
			case EStat.Timid:
				break;
		}
	}

	void PredictionTrack()
	{
		Vector2Int playerPlace = StageManager.Instance.Player.CurrentPlace;
		Direction playerDirection = StageManager.Instance.Player.CurrentDir;
		Vector2Int targetPlace = playerPlace;

		for (int i = 1; i < PredictionWeight + 1; i++)
		{
			Vector2Int checkPlace = playerPlace;
			checkPlace.x += (int)playerDirection.X * i;
			checkPlace.y += (int)playerDirection.Y * i;
			if (StageManager.Instance.CanMovePlace(checkPlace))
			{
				targetPlace = checkPlace;
			}
			else
			{
				break;
			}
		}

		SetTargetPlace(targetPlace);
	}

	void DirectTrack()
	{
		Vector2Int targetPlace = StageManager.Instance.Player.CurrentPlace;

		SetTargetPlace(targetPlace);
	}

	protected override void SetNextPlace()
	{
		CurrentPlace = Util.RoundToVectorInt((Vector2)transform.position);

		waypointQueue.Clear();
		ActiveTracking();

		if (waypointQueue.Count > 0)
		{
			Vector2Int nextPlace = waypointQueue.Dequeue();
			moveHandler.SetDestination(CurrentPlace, nextPlace);
			UpdateAnimation(nextPlace);
		}
	}
}
