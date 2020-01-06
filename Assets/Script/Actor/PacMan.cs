using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : Actor
{
	[SerializeField]
	float moveSpeed = 1.0f;

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
			Debug.Log("Reserv Dir Called");
		}
		else if (CheckDirectionPlace(currentDir, ref nextPlace))
		{
			moveHandler.SetDestination(currentPlace, nextPlace);
			Debug.Log("Next Dir Called");
		}
		else
		{
			currentDir = Direction.Empty;
			reservDir = Direction.Empty;
			animHandler.StopAnimation();
			Debug.Log("Blocked Called");
		}
	}

	bool CheckDirectionPlace(Direction dir, ref Vector3Int nextPlace)
	{
		nextPlace.x += (int)dir.X;
		nextPlace.y += (int)dir.Y;

		if (StageManager.Instance.CanMovePlace(nextPlace))
		{
			return true;
		}
		return false;
	}

}
