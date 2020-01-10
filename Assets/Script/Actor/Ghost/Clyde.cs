using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clyde : Ghost
{
	void Awake()
	{
		gizmoColor = Color.yellow;
	}

	// 랜덤 위치로 이동
	protected override void UpdateActionDecision()
	{
		if (!StageManager.Instance.CanMovePlace(CurrentPlace))
		{
			SetPlace(startPlace);
		}

		Vector2Int targetPlace;

		if (waypointQueue.Count == 0)
		{
			switch (currentState)
			{
				case EState.Warp:
					if (waypointQueue.Count == 0)
					{
						if (currentState != EState.Timid)
						{
							currentState = EState.Normal;
						}
					}
					break;
				case EState.Timid:
					targetPlace = FindTimidRunPlace(new Direction(EDirX.Left, EDirY.Down));
					SetTargetPlace(targetPlace);
					break;
				case EState.GoHome:
					SetTargetPlace(homePlace);
					break;
				default:
					targetPlace = FindRandomPlace();
					SetTargetPlace(targetPlace);
					break;
			}
		}
	}

	Vector2Int FindRandomPlace()
	{
		Vector2Int randomPlace = new Vector2Int();

		do
		{
			randomPlace.x = Random.Range(stageMin.x, stageMax.x);
			randomPlace.y = Random.Range(stageMin.y, stageMax.y);

			if (StageManager.Instance.CanMovePlace(randomPlace))
			{
				break;
			}

		} while (true);

		return randomPlace;
	}

}
