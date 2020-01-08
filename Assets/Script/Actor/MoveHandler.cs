using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHandler : MonoBehaviour
{

	float moveTime = 0.0f;
	Vector2Int startPlace;
	Vector2Int destPlace;

	public Action arriveEvent;

	public float MoveSpeed { get; set; }

	public void Init(float moveSpeed, Action arrived)
	{
		MoveSpeed = moveSpeed;
		arriveEvent += arrived;
	}

	public void Move()
	{
		moveTime += Time.deltaTime * MoveSpeed;

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
