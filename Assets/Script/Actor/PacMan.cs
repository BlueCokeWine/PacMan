#pragma warning disable CS0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : Actor
{
	[SerializeField]
	float moveSpeed;

	Camera mainCamera;
	MoveHandler moveHandler;
	PacManAnimationHandler animHandler;

	public Direction CurrentDir { get; private set; }
	Direction reservDir;
	Vector2Int startPlace;

	public void Init(Vector2Int startPlace)
	{
		mainCamera = Camera.main;

		moveHandler = GetComponent<MoveHandler>();
		animHandler = GetComponent<PacManAnimationHandler>();
		moveHandler.Init(moveSpeed, SetNextPlace);

		this.startPlace = startPlace;
		CurrentDir = Direction.Empty;
		reservDir = Direction.Empty;

		SetPlace(startPlace);
	}

	public override void ResetData()
	{
		CurrentDir = Direction.Empty;
		reservDir = Direction.Empty;

		animHandler.ResetParam();

		SetPlace(startPlace);
	}

	public void Die()
	{
		animHandler.SetDie();
	}

	void Update()
	{
		if (StageManager.Instance.GameState == StageManager.EState.Play)
		{
			KeyBoardInput();

			if (CurrentDir != Direction.Empty && !StageManager.Instance.IsHighlightTime)
			{
				moveHandler.Move();
			}
		}
	}

	void KeyBoardInput()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			SetReservDirection(EDirX.None, EDirY.Up);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			SetReservDirection(EDirX.None, EDirY.Down);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			SetReservDirection(EDirX.Right, EDirY.None);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			SetReservDirection(EDirX.Left, EDirY.None);
		}
	}

	public void JoyStickInput(EDirX x, EDirY y)
	{
		if (StageManager.Instance.GameState == StageManager.EState.Play)
		{
			SetReservDirection(x, y);
		}
	}

	void SetReservDirection(EDirX x, EDirY y)
	{
		reservDir.X = x;
		reservDir.Y = y;

		if (CurrentDir == Direction.Empty && reservDir != Direction.Empty)
		{
			SetNextPlace();
		}
	}

	void SetNextPlace()
	{
		CurrentPlace = Util.RoundToVectorInt((Vector2)transform.position);
		Vector2Int nextPlace = CurrentPlace;

		if (reservDir != Direction.Empty && CheckDirectionPlace(reservDir, ref nextPlace))
		{
			CurrentDir = reservDir;
			reservDir = Direction.Empty;
			moveHandler.SetDestination(CurrentPlace, nextPlace);
			animHandler.SetDirection(CurrentDir);
		}
		else if (CurrentDir != Direction.Empty && CheckDirectionPlace(CurrentDir, ref nextPlace))
		{
			moveHandler.SetDestination(CurrentPlace, nextPlace);
		}
		else
		{
			CurrentDir = Direction.Empty;
			reservDir = Direction.Empty;
			//animHandler.StopAnimation();
		}
	}

	bool CheckDirectionPlace(Direction dir, ref Vector2Int nextPlace)
	{
		Vector2Int tempPlace = nextPlace;
		tempPlace.x += (int)dir.X;
		tempPlace.y += (int)dir.Y;

		if (StageManager.Instance.CanMovePlace(tempPlace))
		{
			nextPlace = tempPlace;
			return true;
		}
		return false;
	}

}
