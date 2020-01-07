using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : Actor
{
	[SerializeField]
	float moveSpeed;

	Camera mainCamera;
	MoveHandler moveHandler;
	AnimationHandler animHandler;

	Direction currentDir;
	Direction reservDir;

	void Awake()
	{
		mainCamera = Camera.main;

		moveHandler = GetComponent<MoveHandler>();
		animHandler = GetComponent<AnimationHandler>();
		moveHandler.Init(moveSpeed, SetNextPlace);

		currentDir = Direction.Empty;
		reservDir = Direction.Empty;
	}

	void Update()
	{
		KeyInput();

		if(currentDir != Direction.Empty)
		{
			moveHandler.Move();
		}

		SyncCameraPosition();
	}

	void KeyInput()
	{
		MoveKeyInput();
	}

	void MoveKeyInput()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			reservDir.X = EDirX.None;
			reservDir.Y = EDirY.Up;
		}

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			reservDir.X = EDirX.None;
			reservDir.Y = EDirY.Down;
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			reservDir.X = EDirX.Right;
			reservDir.Y = EDirY.None;
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			reservDir.X = EDirX.Left;
			reservDir.Y = EDirY.None;
		}

		if(currentDir == Direction.Empty && reservDir != Direction.Empty)
		{
			SetNextPlace();
		}
	}

	void SyncCameraPosition()
	{
		Vector3 position = mainCamera.transform.position;

		position.x = transform.position.x;
		position.y = transform.position.y;

		mainCamera.transform.position = position;
	}

	void SetNextPlace()
	{
		currentPlace = Util.RoundToVectorInt(transform.position);
		Vector3Int nextPlace = currentPlace;

		if (reservDir != Direction.Empty && CheckDirectionPlace(reservDir, ref nextPlace))
		{
			currentDir = reservDir;
			reservDir = Direction.Empty;
			moveHandler.SetDestination(currentPlace, nextPlace);
			animHandler.SetDirection(currentDir);
		}
		else if (currentDir != Direction.Empty && CheckDirectionPlace(currentDir, ref nextPlace))
		{
			moveHandler.SetDestination(currentPlace, nextPlace);
		}
		else
		{
			currentDir = Direction.Empty;
			reservDir = Direction.Empty;
			animHandler.StopAnimation();
		}
	}

	bool CheckDirectionPlace(Direction dir, ref Vector3Int nextPlace)
	{
		Vector3Int tempPlace = nextPlace;
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
