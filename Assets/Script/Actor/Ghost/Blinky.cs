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

	protected override void ActiveTracking()
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
