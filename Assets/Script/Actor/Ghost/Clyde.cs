using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
{
	void Awake()
	{
		gizmoColor = Color.yellow;
	}

	protected override void ActiveTracking()
	{
		Vector2Int targetPlace = new Vector2Int();

		do
		{
			targetPlace.x = Random.Range(stageMin.x, stageMax.x);
			targetPlace.y = Random.Range(stageMin.y, stageMax.y);

			if (StageManager.Instance.CanMovePlace(targetPlace))
			{
				break;
			}

		} while (true);

		SetTargetPlace(targetPlace);
	}

	protected override void SetNextPlace()
	{
		CurrentPlace = Util.RoundToVectorInt((Vector2)transform.position);

		if(waypointQueue.Count <= 1)
		{
			ActiveTracking();
		}

		if (waypointQueue.Count > 0)
		{
			Vector2Int nextPlace = waypointQueue.Dequeue();
			moveHandler.SetDestination(CurrentPlace, nextPlace);
			UpdateAnimation(nextPlace);
		}
		
	}
}
