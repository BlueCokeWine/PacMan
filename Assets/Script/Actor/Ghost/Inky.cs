using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
	const int MaxDistanceWeight = 5;

	Blinky blinky;

	void Awake()
	{
		gizmoColor = Color.cyan;
	}

	// 블링키와 플레이어의 대칭점으로 이동하려고 함
	protected override void UpdateActionDecision()
	{
		Vector2Int targetPlace;

		if (waypointQueue.Count == 0)
		{

			switch (currentState)
			{
				case EState.Warp:
					if (waypointQueue.Count == 0)
					{
						currentState = EState.Normal;
					}
					break;
				case EState.Timid:
					targetPlace = FindTimidRunPlace(new Direction(EDirX.Right, EDirY.Down));
					SetTargetPlace(targetPlace);
					break;
				case EState.GoHome:
					SetTargetPlace(homePlace);
					break;
				default:
					targetPlace = FindSymmetryPlace();
					SetTargetPlace(targetPlace);
					break;
			}
		}
	}

	Vector2Int FindSymmetryPlace()
	{
		Vector2Int targetPlace = new Vector2Int();
		Vector2Int symmetryPlace = new Vector2Int();
		Vector2Int centerPlace = StageManager.Instance.Player.CurrentPlace;
		symmetryPlace.x = SymmetryValue(CurrentPlace.x, centerPlace.x);
		symmetryPlace.y = SymmetryValue(CurrentPlace.y, centerPlace.y);

		for(int i = 0; i < MaxDistanceWeight; i++)
		{
			for(int j = 0; j < MaxDistanceWeight; j++)
			{
				targetPlace.x = symmetryPlace.x + i;
				targetPlace.y = symmetryPlace.y + i;

				if (StageManager.Instance.CanMovePlace(targetPlace))
				{
					return targetPlace;
				}

				targetPlace.x = symmetryPlace.x - i;
				targetPlace.y = symmetryPlace.y - i;

				if (StageManager.Instance.CanMovePlace(targetPlace))
				{
					return targetPlace;
				}
			}
		}

		return centerPlace;
	}

	int SymmetryValue(int original, int center)
	{
		int symmetry = 0;
		symmetry = ((center - original) * 2) + original;
		return symmetry;
	}

}
