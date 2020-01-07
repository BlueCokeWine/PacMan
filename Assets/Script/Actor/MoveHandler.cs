using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{

	float moveSpeed;
	float moveTime = 0.0f;
	Vector2Int startPlace;
	Vector2Int destPlace;

	public Action arriveEvent;

	public void Init(float moveSpeed, Action arrived)
	{
		this.moveSpeed = moveSpeed;
		arriveEvent += arrived;
	}

	public void Move()
	{
		moveTime += Time.deltaTime * moveSpeed;

		var position = transform.position;
		position.x = Mathf.Lerp(startPlace.x, destPlace.x, moveTime);
		position.y = Mathf.Lerp(startPlace.y, destPlace.y, moveTime);
		transform.position = position;

		if (moveTime > 1.0f)
		{
			arriveEvent();
			moveTime = 0.0f;
		}
	}

	public void SetDestination(Vector2Int currentPlace, Vector2Int nextPlace)
	{
		startPlace = currentPlace;
		destPlace = nextPlace;
	}

}
